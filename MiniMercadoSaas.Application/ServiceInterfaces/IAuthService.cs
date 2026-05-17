using MiniMercadoSaas.Application.DTO.Request;
using MiniMercadoSaas.Application.DTO.Response;

namespace MiniMercadoSaas.Application.ServiceInterfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
}