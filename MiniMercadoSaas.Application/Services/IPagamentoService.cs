using MiniMercadoSaas.Application.DTO.Response;

namespace MiniMercadoSaas.Application.Services;

public interface IPagamentoService
{
    Task<PixResponse> GerarPagamentoPix(Guid vendaId, decimal valor);
}