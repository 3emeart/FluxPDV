using MiniMercadoSaas.Domain.Entities;

namespace MiniMercadoSaas.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task AddAsync (User user);
    Task UpdateAsync (User user);
    Task DeleteAsync(User user);
    
    Task <IEnumerable<User>> GetAllAsync();
    
}
