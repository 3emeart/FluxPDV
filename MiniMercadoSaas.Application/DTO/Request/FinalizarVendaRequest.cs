using MiniMercadoSaas.Domain.Enums;

namespace MiniMercadoSaas.Application.DTO.Request;

public class FinalizarVendaRequest
{
    public FormaPagamento FormaPagamento { get; set; }
    public decimal? ValorPago { get; set; }
}
