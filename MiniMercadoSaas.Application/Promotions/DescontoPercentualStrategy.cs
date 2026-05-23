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

        // ValorDesconto representa a taxa decimal (ex: 0.10 para 10% de desconto)
        decimal percentualDesconto = regra.ValorDesconto.Value;
        decimal valorDescontoTotal = (item.PrecoUnitario * item.Quantidade) * percentualDesconto;

        item.Subtotal = (item.PrecoUnitario * item.Quantidade) - valorDescontoTotal;
    }
}
