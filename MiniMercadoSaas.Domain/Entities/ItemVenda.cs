namespace MiniMercadoSaas.Domain.Entities;

public class ItemVenda
{
    protected ItemVenda() { }

    public ItemVenda(int produtoId, int quantidade, decimal precoUnitario)
    {
        Id = Guid.NewGuid();
        ProdutoId = produtoId;
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
        Subtotal = quantidade * precoUnitario;
    }

    public Guid Id { get; set; }
    public Guid VendaId { get; set; }
    public Venda Venda { get; set; }
    public int ProdutoId { get; set; }
    public Produto Produto { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Subtotal { get; set; }
}