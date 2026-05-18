using MiniMercadoSaas.Domain.Enums;

namespace MiniMercadoSaas.Domain.Entities;

public class Venda  
{
    public Guid Id { get; set; }
    public Guid OperadorId { get; set; }
    public Role Operador { get; set; }
    public StatusVenda Status { get; set; }
    public FormaPagamento FormaPagamento { get; set; }
    public decimal TotalFinal { get; set; } = decimal.Zero;
    public string? MotivoCancelamento { get; set; }
    public DateTime AbertaEm { get; set; } = DateTime.Today;
    public DateTime? FinalizadaEm { get; set; } 
    public DateTime? CanceladaEm { get; set; }
    public ICollection<ItemVenda>? Itens { get; set; } = new List<ItemVenda>();

    
}