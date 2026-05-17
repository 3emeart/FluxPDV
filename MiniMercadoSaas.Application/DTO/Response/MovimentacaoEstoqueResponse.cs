using MiniMercadoSaas.Domain.Enums;

namespace MiniMercadoSaas.Application.DTO.Response;

public class MovimentacaoEstoqueResponse
{
    public Guid Id { get; set; }
    public int ProdutoId { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public TipoMovimentacao Tipo { get; set; }
    public Guid? VendaId { get; set; }
    public string? Observacao { get; set; }
    public DateTime CriadoEm { get; set; }
}
