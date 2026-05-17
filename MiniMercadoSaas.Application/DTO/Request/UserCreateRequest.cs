using MiniMercadoSaas.Domain.Enums;

namespace MiniMercadoSaas.Application.DTO.Request;

public class UserCreateRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
}