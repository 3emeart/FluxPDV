using MiniMercadoSaas.Domain.Enums;

namespace MiniMercadoSaas.Application.DTO.Response;

public class VendaResponse(Guid vendaId, decimal vendaTotalFinal, DateTime vendaAbertaEm, StatusVenda vendaStatus, Guid vendaOperadorId)
{
    public Guid Id { get; set; } = vendaId;
    public Guid OperadorId { get; set; } = vendaOperadorId;
    public decimal TotalFinal { get; set; } = vendaTotalFinal;
    public DateTime AbertaEm { get; set; } = vendaAbertaEm;
    public StatusVenda Status { get; set; } = vendaStatus;
}