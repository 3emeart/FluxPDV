using FluentAssertions;
using MiniMercadoSaas.Application.DTO.Request;
using MiniMercadoSaas.Application.ServiceInterfaces;
using MiniMercadoSaas.Application.Services;
using MiniMercadoSaas.Domain.Entities;
using MiniMercadoSaas.Domain.Enums;
using MiniMercadoSaas.Domain.Interfaces;
using Moq;

namespace TestProject1;

public class AuthServiceTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepoMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _usuarioRepoMock = new Mock<IUsuarioRepository>();
        _jwtServiceMock = new Mock<IJwtService>();
        _authService = new AuthService(_usuarioRepoMock.Object, _jwtServiceMock.Object);
    }

    // TESTE DE EMAIL INEXISTENE
    [Fact]
    public async Task LoginAsync_ComEmailInexistente_DeveLancarUnauthorizedException()
    {
        var request = new LoginRequest { Email = "naoexiste@teste.com", Password = "123" };
        _usuarioRepoMock.Setup(r => r.GetByEmailAsync(request.Email)).ReturnsAsync((User)null);

        var act = () => _authService.LoginAsync(request);

        
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Credenciais Inválidas");
    }
    
    //TESTE DE SENHA INCORRETA

    [Fact]
    public async Task LoginAsync_ComSenhaIncorreta_DeveLancarUnauthorizedExceptionComMesmaMensagem()
    {
        var senhaCorreta = "senha_correta";
        var senhaErrada = "senha_errada";
        var hashCorreto = BCrypt.Net.BCrypt.HashPassword(senhaCorreta);

        var usuario = new User { Email = "teste@teste.com", SenhaHash = hashCorreto };
        var request = new LoginRequest { Email = usuario.Email, Password = senhaErrada };

        _usuarioRepoMock.Setup(r => r.GetByEmailAsync(request.Email)).ReturnsAsync(usuario);

        var act = () => _authService.LoginAsync(request);

        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Credenciais Inválidas");
    }
    
    //TESTE DE SUCESSO COM CREDENCIAIS CORRETAS

    [Fact]
    public async Task LoginAsync_ComCredenciaisCorretas_DeveRetornarLoginResponseValida()
    {
        var senha = "senha_valida";
        var hash = BCrypt.Net.BCrypt.HashPassword(senha);
        var usuario = new User 
        { 
            Id = Guid.NewGuid(), 
            Email = "sucesso@teste.com", 
            SenhaHash = hash,
            Name = "Teste Sucesso",
            Role = Role.Admin 
        };

        var request = new LoginRequest { Email = usuario.Email, Password = senha };
        var tokenEsperado = "fake-jwt-token";

        _usuarioRepoMock.Setup(r => r.GetByEmailAsync(request.Email)).ReturnsAsync(usuario);
        _jwtServiceMock.Setup(j => j.GenerateToken(usuario)).Returns(tokenEsperado);

        var response = await _authService.LoginAsync(request);

        response.Should().NotBeNull();
        response.Token.Should().Be(tokenEsperado);
        response.Expires.Should().BeAfter(DateTime.UtcNow);
        response.UserData.Email.Should().Be(usuario.Email);
        
        _usuarioRepoMock.Verify(r => r.UpdateAsync(It.Is<User>(u => u.Email == usuario.Email)), Times.Once);
    }
}