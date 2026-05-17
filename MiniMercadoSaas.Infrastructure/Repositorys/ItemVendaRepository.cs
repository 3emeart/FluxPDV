using MiniMercadoSaas.Domain.Entities;
using MiniMercadoSaas.Domain.Interfaces;
using MiniMercadoSaas.Infrastructure.Context;

namespace MiniMercadoSaas.Infrastructure.Repositorys;

public class ItemVendaRepository : IItemVendaRepository
{
    private readonly AppDbContext _dbContext;
    
    public ItemVendaRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(ItemVenda item)
    {
        await _dbContext.AddAsync(item);
    }

    public async Task DeleteAsync(ItemVenda item)
    {
         _dbContext.Remove(item);
    }
    
}