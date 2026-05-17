using Microsoft.EntityFrameworkCore;
using MiniMercadoSaas.Domain.Entities;
using MiniMercadoSaas.Domain.Interfaces;
using MiniMercadoSaas.Infrastructure.Context;

namespace MiniMercadoSaas.Infrastructure.Repositorys;

public class VendaRepository : IVendaRepository
{
    private readonly AppDbContext _dbContext;

    public VendaRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Venda?> GetByIdAsync(Guid id, bool includeItens = true)
    {
        IQueryable<Venda> query = _dbContext.Vendas;

        if (includeItens)
        {
            query = query.Include(v => v.Itens)
                .ThenInclude(i => i.Produto);

        }

        return await query.FirstOrDefaultAsync(v => v.Id == id);

    }

    public async Task<IEnumerable<Venda>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim, bool includeItens)
    {
        IQueryable<Venda> query = _dbContext.Vendas;

        if (includeItens)
        {
            query = query.Include(v => v.Itens)
                .ThenInclude(i => i.Produto);
        }

        return await query
            .Where(v => ((v.FinalizadaEm ?? v.CanceladaEm) ?? v.AbertaEm) >= inicio
                        && ((v.FinalizadaEm ?? v.CanceladaEm) ?? v.AbertaEm) <= fim)
            .OrderByDescending(v => (v.FinalizadaEm ?? v.CanceladaEm) ?? v.AbertaEm)
            .ToListAsync();
    }

    public async Task AddAsync(Venda venda)
    {
        await _dbContext.Vendas.AddAsync(venda);
    }

    public async Task UpdateAsync(Venda venda)
    {
        var entry = _dbContext.Entry(venda);

        if (entry.State == EntityState.Detached)
        {
            _dbContext.Vendas.Update(venda);
        }

        try 
        {
        }
        catch (DbUpdateConcurrencyException)
        {
            
        }
    }

   

   
}
