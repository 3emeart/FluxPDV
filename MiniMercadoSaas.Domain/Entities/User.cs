using MiniMercadoSaas.Domain.Enums;

namespace MiniMercadoSaas.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public  string Name { get; set; } = string.Empty;
    public required string Email { get; set; }
    public string SenhaHash { get; set; } = string.Empty;
    public bool Active { get; set; }
    public Role Role { get; set; }
    public DateTime CreatedIn { get; set; } = DateTime.UtcNow;
    public DateTime UltimoLoginIn { get; set; } 
}