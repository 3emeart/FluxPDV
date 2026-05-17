using Microsoft.EntityFrameworkCore;
using MiniMercadoSaas.Domain.Entities;
using MiniMercadoSaas.Domain.Interfaces;
using MiniMercadoSaas.Infrastructure.Context;

namespace MiniMercadoSaas.Infrastructure.Repositorys;

public class MovimentacaoEstoqueRepository : IMovimentacaoEstoqueRepository
{
    private readonly AppDbContext _dbContext;

    public MovimentacaoEstoqueRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(MovimentacaoEstoque movimentacao)
    {
        await _dbContext.MovimentacoesEstoque.AddAsync(movimentacao);
    }

    public async Task<IEnumerable<MovimentacaoEstoque>> GetByProdutoIdAsync(int produtoId)
    {
        return await _dbContext.MovimentacoesEstoque
            .Include(m => m.Produto)
            .Where(m => m.ProdutoId == produtoId)
            .OrderByDescending(m => m.CriadoEm)
            .ToListAsync();
    }
}
