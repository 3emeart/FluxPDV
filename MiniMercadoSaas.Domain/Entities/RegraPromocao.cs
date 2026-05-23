namespace MiniMercadoSaas.Domain.Entities;

public class RegraPromocao
{

    public Guid Id { get; set; }
    public Guid PromocaoId { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public Promocao Promocao { get; set; } = null!;
    
    public int ProdutoId { get; set; }
    
    [System.Text.Json.Serialization.JsonIgnore]
    public Produto Produto { get; set; } = null!;

    public int QuantidadeMinima { get; set; }

    public decimal? ValorDesconto { get; set; }

    public int? QuantidadePaga { get; set; }

    protected RegraPromocao()
    {
    }

    public RegraPromocao(Guid promocaoId, int produtoId, int quantidadeMinima, decimal? valorDesconto,
        int? quantidadePaga)
    {
        Id = Guid.NewGuid();
        PromocaoId = promocaoId;
        ProdutoId = produtoId;
        QuantidadeMinima = quantidadeMinima;
        ValorDesconto = valorDesconto;
        QuantidadePaga = quantidadePaga;
    }

}
