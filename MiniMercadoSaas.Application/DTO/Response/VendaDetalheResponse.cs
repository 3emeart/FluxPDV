using MiniMercadoSaas.Domain.Entities;
using MiniMercadoSaas.Domain.Enums;

namespace MiniMercadoSaas.Application.DTO.Response;

public class VendaDetalheResponse(Venda venda)
{
   

    public Guid Id { get; set; } = venda.Id;
    public StatusVenda StatusVenda { get; set; } = venda.Status; 
    public decimal TotalFinal { get; set; } = venda.TotalFinal;
    public DateTime AbertaEm { get; set; } = venda.AbertaEm; 
    public string? PixCopiaECola {get; set;} 
    public string? PixQrCodebase64 {get; set;}
    
    
    public ICollection<ItemVendaResponse> Itens { get; set; } = venda.Itens.Select(i => new ItemVendaResponse 
    {
        Id = i.Id,
        NomeProduto = i.Produto.Nome,
        Quantidade = i.Quantidade,
        PrecoUnitario = i.PrecoUnitario,
        Subtotal = i.Quantidade * i.PrecoUnitario
    }).ToList();
}

