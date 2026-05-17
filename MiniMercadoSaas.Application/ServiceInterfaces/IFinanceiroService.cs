using MiniMercadoSaas.Application.DTO.Response;

namespace MiniMercadoSaas.Application.ServiceInterfaces;

public interface IFinanceiroService
{
    Task<FinanceiroResumoResponse> ObterResumoAsync(DateTime? inicio, DateTime? fim);
}
