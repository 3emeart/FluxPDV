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

        // O ValorDesconto representa o novo preço unitário reduzido
        decimal novoPrecoUnitario = regra.ValorDesconto.Value;
        item.Subtotal = item.Quantidade * novoPrecoUnitario;
    }
}
