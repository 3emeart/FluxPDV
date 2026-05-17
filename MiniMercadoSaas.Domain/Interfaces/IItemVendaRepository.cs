using MiniMercadoSaas.Domain.Entities;

namespace MiniMercadoSaas.Domain.Interfaces;

public interface IItemVendaRepository
{
    Task AddAsync (ItemVenda item);
    Task DeleteAsync (ItemVenda item);
}