using MiniMercadoSaas.Application.DTO.Request;
using MiniMercadoSaas.Application.DTO.Response;
using MiniMercadoSaas.Domain.Entities;

namespace MiniMercadoSaas.Application.ServiceInterfaces;

public interface ICategoryService
{
    Task<CategoriaResponse> BuscarCategoriaPorNome(string nome);
    Task<CategoriaResponse> CriarCategoria(CategoriaCreateRequest request);
    Task<CategoriaResponse> AtualizarCategoria(int id, CategoriaCreateRequest request);
    Task DeletarCategoria(int id);
    
    Task<IEnumerable<CategoriaResponse>> ListarCategorias();
    
}