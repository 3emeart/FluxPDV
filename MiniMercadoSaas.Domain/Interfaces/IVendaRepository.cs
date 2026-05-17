using MiniMercadoSaas.Domain.Entities;

namespace MiniMercadoSaas.Domain.Interfaces;

public interface IVendaRepository
{
    Task<Venda> GetByIdAsync(Guid vendaId, bool includeItens);
    Task<IEnumerable<Venda>> ListarPorPeriodoAsync(DateTime inicio, DateTime fim, bool includeItens);
    Task AddAsync(Venda venda);
    Task UpdateAsync(Venda venda);
}
