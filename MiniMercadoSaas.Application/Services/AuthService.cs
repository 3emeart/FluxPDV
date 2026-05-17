using MiniMercadoSaas.Application.DTO.Request;
using MiniMercadoSaas.Application.DTO.Response;
using MiniMercadoSaas.Application.ServiceInterfaces;
using MiniMercadoSaas.Domain.Interfaces;
using MiniMercadoSaas.Domain.Entities;

namespace MiniMercadoSaas.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IJwtService _jwtService;

    public AuthService(IUsuarioRepository usuarioRepository, IJwtService jwtService)
    {
        _usuarioRepository = usuarioRepository;
        _jwtService = jwtService;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
        var usuario = await _usuarioRepository.GetByEmailAsync(loginRequest.Email);
        if (usuario == null)
        {
            throw new UnauthorizedAccessException("Credenciais Inválidas");
        }

        if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, usuario.SenhaHash))

            throw new UnauthorizedAccessException("Credenciais Inválidas");

        usuario.UltimoLoginIn = DateTime.UtcNow;
        await _usuarioRepository.UpdateAsync(usuario);

        var token = _jwtService.GenerateToken(usuario);
        return new LoginResponse
        {
            Token = token,
            Expires = DateTime.UtcNow.AddDays(8),
            UserData = new LoginResponse.UserDataResponse
            {
                Email = usuario.Email,
                Id = usuario.Id,
                Nome = usuario.Name,
                Role = usuario.Role.ToString()
            }



        };
    }

}
        