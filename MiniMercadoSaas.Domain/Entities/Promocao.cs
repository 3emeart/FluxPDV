using MiniMercadoSaas.Domain.Enums;

namespace MiniMercadoSaas.Domain.Entities;

public class Promocao
{
    protected Promocao () {}

    public Promocao(string nome, string descricao, TipoPromocao tipo, DateTime dataInicio, DateTime dataFim)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Descricao = descricao;
        Tipo = tipo;
        DataInicio = dataInicio;
        DataFim = dataFim;
        Ativo = true;
    }
    
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public TipoPromocao Tipo { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public bool Ativo { get; set; }
    
    public ICollection<RegraPromocao> Regras { get; set; } = new List<RegraPromocao>();
    public bool EstaValida() => Ativo && DateTime.UtcNow >= DataInicio && DateTime.UtcNow <= DataFim;

    public static System.Linq.Expressions.Expression<Func<Promocao, bool>> QueryAtivas(DateTime data)
        => p => p.Ativo && p.DataInicio <= data && p.DataFim >= data;
}
