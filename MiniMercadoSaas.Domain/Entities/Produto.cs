namespace MiniMercadoSaas.Domain.Entities;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    
    public int Quantidade { get; set; }
    public decimal PrecoVenda { get; set; }
    public decimal PrecoCompra { get; set; }
    public bool Ativo { get; set; } = true;
    public int EstoqueMinimo { get; set; } = 5;
    public enum MedidaVenda 
        {
         Unidade,
         Kg,
         
        }
    public required Categoria? Categoria { get; set; }
    public int CategoriaId { get; set; }
    public ICollection<MovimentacaoEstoque> Movimentacoes { get; set; } = new List<MovimentacaoEstoque>();
}