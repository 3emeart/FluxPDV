using MiniMercadoSaas.Domain.Enums;

namespace MiniMercadoSaas.Application.DTO.Request;

public class PromocaoRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public TipoPromocao Tipo { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public List<RegraPromocaoRequest> Regras { get; set; } = [];
}

public class RegraPromocaoRequest
{
    public int ProdutoId { get; set; }
    public int QuantidadeMinima { get; set; }
    public decimal? ValorDesconto { get; set; }
    public int? QuantidadePaga { get; set; }
}
