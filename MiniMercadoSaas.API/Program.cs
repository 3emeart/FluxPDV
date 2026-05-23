using System.Text;
using Microsoft.EntityFrameworkCore;
using MiniMercadoSaas.Application.ServiceInterfaces;
using MiniMercadoSaas.Application.Services;
using MiniMercadoSaas.Domain;
using MiniMercadoSaas.Infrastructure.Context;
using MiniMercadoSaas.Infrastructure.Repositorys;
using MiniMercadoSaas.Application.Promotions;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MiniMercadoSaas.Application.Validators;
using MiniMercadoSaas.Domain.Interfaces;
using MiniMercadoSaas.Infrastructure.Consumers;
using MiniMercadoSaas.API.Hubs;
using MiniMercadoSaas.API.Services;

var builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsDevelopment())
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";

    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.ListenAnyIP(int.Parse(port));
    });
}

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") 
                       ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") 
                ?? builder.Configuration["JwtSettings:SecretKey"];

var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") 
                ?? builder.Configuration["JwtSettings:Issuer"];

var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") 
                  ?? builder.Configuration["JwtSettings:Audience"];

if (!string.IsNullOrEmpty(secretKey))
{
    var key = Encoding.ASCII.GetBytes(secretKey);

    builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                ValidateLifetime = true,
            };
        });
}

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MineMercado SaaS API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira apenas o token JWT no campo abaixo. Exemplo: eyJhbG..."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IEstoqueAlertaNotificador, SignalREstoqueAlertaNotificador>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUsuarioRepository, UserRepository>();
builder.Services.AddScoped<IVendaRepository, VendaRepository>();
builder.Services.AddScoped<IItemVendaRepository, ItemVendaRepository>();
builder.Services.AddScoped<IMovimentacaoEstoqueRepository, MovimentacaoEstoqueRepository>();
builder.Services.AddScoped<IPromocaoRepository, PromocaoRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IProductService, ProdutoService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IVendaService, VendaService>();
builder.Services.AddScoped<IEstoqueService, EstoqueService>();
builder.Services.AddScoped<IFinanceiroService, FinanceiroService>();

builder.Services.AddScoped<IPromotionStrategy, LeveXPagueYStrategy>();
builder.Services.AddScoped<IPromotionStrategy, DescontoQuantidadeStrategy>();
builder.Services.AddScoped<IPromotionStrategy, DescontoPercentualStrategy>();
builder.Services.AddScoped<IPromotionEngine, PromotionEngine>();

builder.Services.AddValidatorsFromAssemblyContaining<ProductValidator>();

var mensageriaUrl = Environment.GetEnvironmentVariable("MENSAGERIA_URL");

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<EstoqueBaixoConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        if (!string.IsNullOrEmpty(mensageriaUrl))
        {
            cfg.Host(new Uri(mensageriaUrl));
        }
        else
        {
            cfg.Host(new Uri("rabbitmq://localhost"), h =>
            {
                h.Username("guest");
                h.Password("guest");
            });
        }

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("ProductionPolicy", policy =>
    {
        policy.WithOrigins("https://fluxpdvsite.onrender.com")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();

app.UseCors("ProductionPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();
app.MapHub<NotificationHub>("/hubs/notifications");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();