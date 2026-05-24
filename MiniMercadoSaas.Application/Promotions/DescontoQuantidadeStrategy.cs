using MiniMercadoSaas.Domain.Entities;
using MiniMercadoSaas.Domain.Enums;

namespace MiniMercadoSaas.Application.Promotions;

public class DescontoQuantidadeStrategy : IPromotionStrategy
{
    public TipoPromocao Tipo => TipoPromocao.DescontoPorQuantidade;

    public void AplicarDesconto(ItemVenda item, RegraPromocao regra)
    {
        if (item.Quantidade < regra.QuantidadeMinima || !regra.ValorDesconto.HasValue)
            return;

        decimal descontoPorUnidade = regra.ValorDesconto.Value;
        decimal precoComDesconto = Math.Max(0, item.PrecoUnitario - descontoPorUnidade);

        item.Subtotal = item.Quantidade * precoComDesconto;
    }
}
