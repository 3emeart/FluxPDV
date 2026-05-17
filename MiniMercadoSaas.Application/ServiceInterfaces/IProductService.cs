using MiniMercadoSaas.Application.DTO.Request;
using MiniMercadoSaas.Application.DTO.Response;

namespace MiniMercadoSaas.Application.ServiceInterfaces;

public interface IProductService
{
    Task <ProdutoResponse> Create(ProdutoRequest request);
    Task<ProdutoResponse> Update(int id, ProdutoRequest request);
    Task Delete(int id);
    Task<ProdutoResponse> BuscarPorId(int id);
    Task <IEnumerable<ProdutoResponse>> BuscarTodos();
    
}