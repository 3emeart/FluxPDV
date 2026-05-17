using MiniMercadoSaas.Domain.Entities;

namespace MiniMercadoSaas.Domain;

public interface ICategoryRepository
{
    Task<IEnumerable<Categoria>> ListarCategorias();
    Task<Categoria> ObterCategoriaPorId(int id);
    Task AddAsync (Categoria categoria);
    Task UpdateAsync(Categoria categoria);
    Task DeleteAsync(int id);
    
    Task<Categoria> ObterCategoriaPorNome(string nome);
}