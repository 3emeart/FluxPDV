namespace MiniMercadoSaas.Application.DTO.Request;

public class EntradaEstoqueRequest
{
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public string? Observacao { get; set; }
}
