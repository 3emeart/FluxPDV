namespace MiniMercadoSaas.Application.DTO.Response;

public class EstoqueResumoResponse
{
    public int ProdutoId { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public int QuantidadeAtual { get; set; }
    public int EstoqueMinimo { get; set; }
    public bool AbaixoDoMinimo { get; set; }
}
