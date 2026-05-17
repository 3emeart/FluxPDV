using MiniMercadoSaas.Domain.Entities;

namespace MiniMercadoSaas.Application.DTO.Response;

public class CategoriaResponse
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    
}