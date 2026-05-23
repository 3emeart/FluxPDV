using MiniMercadoSaas.Domain.Entities;
using MiniMercadoSaas.Domain.Enums;

namespace MiniMercadoSaas.Application.Promotions;

public interface IPromotionStrategy
{
    TipoPromocao Tipo { get; }
    void AplicarDesconto(ItemVenda item, RegraPromocao regra);
}
