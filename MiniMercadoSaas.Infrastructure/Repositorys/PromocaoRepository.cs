using System.Transactions;
using Microsoft.EntityFrameworkCore;
using MiniMercadoSaas.Domain.Entities;
using MiniMercadoSaas.Domain.Interfaces;
using MiniMercadoSaas.Infrastructure.Context;

namespace MiniMercadoSaas.Infrastructure.Repositorys;

public class PromocaoRepository : IPromocaoRepository
{
    private readonly AppDbContext _context;
    
    public PromocaoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Promocao promocao)
    {
        await _context.Promocaos.AddAsync(promocao);
    }

    public async Task<Promocao?> ObterPromocaoPorIdAsync(Guid id)
    {
        return await _context.Promocaos.Include(p => p.Regras)
            .ThenInclude(p => p.Produto)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Promocao>> ListarPromocaoAtivaAsync(DateTime data)
    {
        return await _context.Promocaos
            .Include(p => p.Regras)
            .Where(Promocao.QueryAtivas(data))
            .ToListAsync();
    }

    public void Update(Promocao promocao)
    {
        _context.Promocaos.Update(promocao);
    }
}