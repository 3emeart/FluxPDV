using MiniMercadoSaas.Domain.Entities;

namespace MiniMercadoSaas.Application.Promotions;

public interface IPromotionEngine
{
    Task ProcessarPromocoesAsync(Venda venda);
}
