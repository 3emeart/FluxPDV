using MiniMercadoSaas.Domain.Entities;

namespace MiniMercadoSaas.Application.DTO.Response;

public class ProdutoResponse
{
    public string Nome { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public int QuantidadeAtual { get; set; }
    public decimal PrecoCompra { get; set; }
    public decimal PrecoVenda { get; set; }
    public Produto.MedidaVenda MedidaVenda { get; set; }
    public int Id { get; set; }
}
