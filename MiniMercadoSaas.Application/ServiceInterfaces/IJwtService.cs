using MiniMercadoSaas.Domain.Entities;

namespace MiniMercadoSaas.Application.ServiceInterfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}