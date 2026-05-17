namespace MiniMercadoSaas.Application.DTO.Response;

public class FinanceiroResumoResponse
{
    public DateTime Inicio { get; set; }
    public DateTime Fim { get; set; }
    public decimal FaturamentoBruto { get; set; }
    public decimal FaturamentoCancelado { get; set; }
    public decimal TicketMedio { get; set; }
    public int VendasFinalizadas { get; set; }
    public int VendasCanceladas { get; set; }
    public int VendasAbertas { get; set; }
    public int ItensVendidos { get; set; }
    public IEnumerable<FinanceiroFormaPagamentoResponse> FormasPagamento { get; set; } = [];
    public IEnumerable<FinanceiroSerieDiaResponse> SerieDiaria { get; set; } = [];
    public IEnumerable<FinanceiroVendaResponse> UltimasVendas { get; set; } = [];
}

public class FinanceiroFormaPagamentoResponse
{
    public int FormaPagamento { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public int Quantidade { get; set; }
}

public class FinanceiroSerieDiaResponse
{
    public DateTime Data { get; set; }
    public decimal Total { get; set; }
    public int Vendas { get; set; }
}

public class FinanceiroVendaResponse
{
    public Guid Id { get; set; }
    public int Status { get; set; }
    public int FormaPagamento { get; set; }
    public decimal TotalFinal { get; set; }
    public DateTime AbertaEm { get; set; }
    public DateTime? FinalizadaEm { get; set; }
    public DateTime? CanceladaEm { get; set; }
    public int Itens { get; set; }
}
