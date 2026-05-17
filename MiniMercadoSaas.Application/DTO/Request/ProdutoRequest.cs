using MiniMercadoSaas.Domain.Entities;

namespace MiniMercadoSaas.Application.DTO.Request;

public class ProdutoRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public int QuantidadeInicial { get; set; }
    public decimal PrecoCompra { get; set; }
    public decimal PrecoVenda { get; set; }
    public Produto.MedidaVenda MedidaVenda { get; set; }
    public int CategoriaId { get; set; } 
    
}
     
    
    
    


