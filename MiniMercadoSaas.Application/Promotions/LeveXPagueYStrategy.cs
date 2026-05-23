using MiniMercadoSaas.Domain.Entities;
using MiniMercadoSaas.Domain.Enums;

namespace MiniMercadoSaas.Application.Promotions;

public class LeveXPagueYStrategy : IPromotionStrategy
{
    public TipoPromocao Tipo => TipoPromocao.LeveXPagueY;

    public void AplicarDesconto(ItemVenda item, RegraPromocao regra)
    {
        if (item.Quantidade < regra.QuantidadeMinima || !regra.QuantidadePaga.HasValue)
            return;

        int lotePromocional = regra.QuantidadeMinima;
        int quantidadePagaNoLote = regra.QuantidadePaga.Value;

        // Calcula quantos lotes fechados o cliente está levando
        int quantidadeLotes = item.Quantidade / lotePromocional;
        int itensForaDoLote = item.Quantidade % lotePromocional;

        // Total de itens que serão cobrados
        int quantidadeCobrada = (quantidadeLotes * quantidadePagaNoLote) + itensForaDoLote;

        item.Subtotal = quantidadeCobrada * item.PrecoUnitario;
    }
}
