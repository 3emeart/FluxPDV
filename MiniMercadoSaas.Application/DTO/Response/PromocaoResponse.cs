using MiniMercadoSaas.Domain.Enums;

namespace MiniMercadoSaas.Application.DTO.Response;

public class PromocaoResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public TipoPromocao Tipo { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public bool Ativo { get; set; }
    public List<RegraPromocaoResponse> Regras { get; set; } = [];
}

public class RegraPromocaoResponse
{
    public Guid Id { get; set; }
    public Guid PromocaoId { get; set; }
    public int ProdutoId { get; set; }
    public int QuantidadeMinima { get; set; }
    public decimal? ValorDesconto { get; set; }
    public int? QuantidadePaga { get; set; }
}
