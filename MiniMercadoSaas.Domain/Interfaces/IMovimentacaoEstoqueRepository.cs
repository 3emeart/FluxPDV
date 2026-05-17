using MiniMercadoSaas.Domain.Entities;

namespace MiniMercadoSaas.Domain.Interfaces;

public interface IMovimentacaoEstoqueRepository
{
    Task AddAsync(MovimentacaoEstoque movimentacao);
    Task<IEnumerable<MovimentacaoEstoque>> GetByProdutoIdAsync(int produtoId);
}
