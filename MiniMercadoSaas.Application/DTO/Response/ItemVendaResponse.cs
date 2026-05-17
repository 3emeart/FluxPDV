namespace MiniMercadoSaas.Application.DTO.Response;

public class ItemVendaResponse
{
    public Guid ProdutoId { get; set; }
    public Guid Id { get; set; }
    public decimal Subtotal { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public decimal PrecoUnitario { get; set; }
    public int Quantidade { get; set; }
    
}

