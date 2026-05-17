using MiniMercadoSaas.Domain.Enums;

namespace MiniMercadoSaas.Domain.Entities;

public class MovimentacaoEstoque
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int ProdutoId { get; set; }
    public Produto Produto { get; set; } = null!;
    public int Quantidade { get; set; }
    public TipoMovimentacao Tipo { get; set; }
    public Guid? VendaId { get; set; }
    public Venda? Venda { get; set; }
    public Guid UsuarioId { get; set; }
    public string? Observacao { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}
