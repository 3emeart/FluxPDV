using Microsoft.EntityFrameworkCore;
using MiniMercadoSaas.Domain;
using MiniMercadoSaas.Domain.Entities;
using MiniMercadoSaas.Infrastructure.Context;

namespace MiniMercadoSaas.Infrastructure.Repositorys;

public class ProductRepository : IProductRepository
{

    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Produto>> ListAsync()
    {
        return await _context.Produtos.ToListAsync();
    }

    public async Task<Produto?> FindByIdAsync(int id)
    {
        return await _context.Produtos.FindAsync(id);
        
    }

    public async Task AddAsync(Produto produto)
    {
        await _context.Produtos.AddAsync(produto);
    }

    public async Task UpdateAsync(Produto produto)
    {
        _context.Produtos.Update(produto);
    }

    public async Task DeleteAsync(int id)
    {
        await _context.Produtos
            .Where(produto => produto.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task<Produto?> FindByCodigo(string codigo)
    {
        return await _context.Produtos.FirstOrDefaultAsync(p => p.Codigo == codigo); 
    }
}
    
  