using MiniMercadoSaas.Application.DTO.Request;
using MiniMercadoSaas.Application.DTO.Response;

namespace MiniMercadoSaas.Application.ServiceInterfaces;

public interface IEstoqueService
{
    Task<MovimentacaoEstoqueResponse> RegistrarEntradaAsync(EntradaEstoqueRequest request, Guid usuarioId);
    Task<IEnumerable<MovimentacaoEstoqueResponse>> ConsultarMovimentacoesAsync(int produtoId);
    Task<IEnumerable<EstoqueResumoResponse>> ConsultarEstoqueBaixoAsync();
}
