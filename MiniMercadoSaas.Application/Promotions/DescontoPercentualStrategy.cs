using MiniMercadoSaas.Domain.Entities;
using MiniMercadoSaas.Domain.Enums;

namespace MiniMercadoSaas.Application.Promotions;

public class DescontoPercentualStrategy : IPromotionStrategy
{
    public TipoPromocao Tipo => TipoPromocao.DescontoPercentual;

    public void AplicarDesconto(ItemVenda item, RegraPromocao regra)
    {
        if (!regra.ValorDesconto.HasValue)
            return;

        decimal percentualDesconto = regra.ValorDesconto.Value / 100m;
        decimal valorDescontoTotal = (item.PrecoUnitario * item.Quantidade) * percentualDesconto;

        item.Subtotal = (item.PrecoUnitario * item.Quantidade) - valorDescontoTotal;
    }
}
