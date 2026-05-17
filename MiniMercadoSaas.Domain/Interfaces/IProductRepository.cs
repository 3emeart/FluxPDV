using MiniMercadoSaas.Domain.Entities;

namespace MiniMercadoSaas.Domain;

public interface IProductRepository
{
    Task<IEnumerable<Produto>> ListAsync();
    Task<Produto?> FindByIdAsync(int id);
    Task AddAsync(Produto produto);
    Task UpdateAsync(Produto produto);
    Task DeleteAsync(int id);
    
    Task<Produto?> FindByCodigo(string codigo);
    
}