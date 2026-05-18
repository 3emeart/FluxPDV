using Microsoft.EntityFrameworkCore;
using MiniMercadoSaas.Domain.Entities;

namespace MiniMercadoSaas.Infrastructure.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Venda> Vendas { get; set; }
    public DbSet<ItemVenda> ItemVendas { get; set; }
    public DbSet<MovimentacaoEstoque> MovimentacoesEstoque { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<Categoria>()
            .Property(c => c.Nome).HasMaxLength(50).IsRequired();
        
        modelBuilder.Entity<Categoria>()
            .HasMany(c => c.Produtos)
            .WithOne(p => p.Categoria)
            .HasForeignKey(p => p.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Produto>()
            .Property(c => c.Nome).HasMaxLength(50).IsRequired();
        
        modelBuilder.Entity<Produto>()
            .Property(produto => produto.Codigo).HasMaxLength(13).IsRequired();
        
        modelBuilder.Entity<Produto>()
            .HasIndex(produto => produto.Codigo).IsUnique();
        
        modelBuilder.Entity<Produto>()
            .Property(produto => produto.PrecoCompra ).HasColumnType("decimal(18,2)").IsRequired();
        
        modelBuilder.Entity<Produto>()
            .Property(produto => produto.PrecoVenda).HasColumnType("decimal(18,2)").IsRequired();
        
        modelBuilder.Entity<User>()
            .HasIndex(user => user.Email).IsUnique();
        
        modelBuilder.Entity<User>()
            .Property(user => user.Email).HasMaxLength(100).IsRequired();
        
        modelBuilder.Entity<User>()
            .Property(user => user.Name).HasMaxLength(100).IsRequired();

        modelBuilder.Entity<User>()
            .Property(user => user.Role).HasConversion<string>().IsRequired();
        
        modelBuilder.Entity<Venda>()
            .Property(venda => venda.TotalFinal).HasPrecision(18, 2);

        modelBuilder.Entity<Venda>()
            .Property(venda => venda.Status).HasConversion<string>();

        modelBuilder.Entity<Venda>()
            .Property(venda => venda.FormaPagamento).HasConversion<string>();

        modelBuilder.Entity<ItemVenda>()
            .HasOne(itemVenda => itemVenda.Venda)
            .WithMany(itemVenda => itemVenda.Itens)
            .HasForeignKey(itemVenda => itemVenda.VendaId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ItemVenda>()
            .HasOne(itemVenda => itemVenda.Produto)
            .WithMany().HasForeignKey(itemVenda => itemVenda.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<ItemVenda>()
            .Property(itemVenda => itemVenda.PrecoUnitario).HasPrecision(18, 2);
        
        modelBuilder.Entity<ItemVenda>()
            .Property(venda => venda.Subtotal).HasPrecision(18, 2);

        modelBuilder.Entity<MovimentacaoEstoque>()
            .HasOne(m => m.Produto)
            .WithMany(p => p.Movimentacoes)
            .HasForeignKey(m => m.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MovimentacaoEstoque>()
            .HasOne(m => m.Venda)
            .WithMany()
            .HasForeignKey(m => m.VendaId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<MovimentacaoEstoque>()
            .Property(m => m.Tipo).HasConversion<string>();

        modelBuilder.Entity<MovimentacaoEstoque>()
            .Property(m => m.Observacao).HasMaxLength(500);
        
        
            
    }
}