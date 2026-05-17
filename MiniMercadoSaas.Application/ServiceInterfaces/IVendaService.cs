using MiniMercadoSaas.Application.DTO.Request;
using MiniMercadoSaas.Application.DTO.Response;
using MiniMercadoSaas.Domain.Entities;

namespace MiniMercadoSaas.Application.ServiceInterfaces;

public interface IVendaService
{
    Task<VendaDetalheResponse> AbrirAsync(Guid OperadorId);
    Task<Venda> AddItemAsync(Guid id, AddItemRequest request, Guid operadorId);
    Task<Venda> RemoveItemAsync(Guid id, Guid itemId);
    Task<VendaDetalheResponse> FinalizarAsync(Guid vendaId, FinalizarVendaRequest request, Guid operadorId);
    Task<VendaDetalheResponse> CancelarAsync(Guid vendaId, CancelarVendaRequest request, Guid gerenteId);
    Task<VendaDetalheResponse> GetByIdAsync(Guid vendaId);
}