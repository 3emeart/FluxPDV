using Microsoft.EntityFrameworkCore;
using MiniMercadoSaas.Domain;
using MiniMercadoSaas.Domain.Entities;
using MiniMercadoSaas.Infrastructure.Context;

namespace MiniMercadoSaas.Infrastructure.Repositorys;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;
    private ICategoryRepository _categoryRepositoryImplementation;
    private ICategoryRepository _categoryRepositoryImplementation1;
    private ICategoryRepository _categoryRepositoryImplementation2;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Categoria>> ListarCategorias()
    {
        return await _context.Categorias.Include(categoria => categoria.Produtos).ToListAsync();
    }
    
    public async Task<Categoria?> ObterCategoriaPorId(int id)
    {
        return await _context.Categorias.FindAsync(id);
        
    }

    public async Task AddAsync(Categoria categoria)
    {
        await _context.Categorias.AddAsync(categoria);
    }

    public async Task UpdateAsync(Categoria categoria)
    {
        _context.Categorias.Update(categoria);
    }

    public async Task DeleteAsync(int id)
    {
        await _context.Categorias
            .Where(categoria => categoria.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task<Categoria?> ObterCategoriaPorNome(string nome)
    {
        return await _context.Categorias.AsNoTracking().FirstOrDefaultAsync(categoria => categoria.Nome == nome);
    }
}