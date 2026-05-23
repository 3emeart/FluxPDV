using MiniMercadoSaas.Domain.Entities;
using MiniMercadoSaas.Domain.Interfaces;

namespace MiniMercadoSaas.Application.Promotions;

public class PromotionEngine : IPromotionEngine
{
    private readonly IPromocaoRepository _promocaoRepository;
    private readonly IEnumerable<IPromotionStrategy> _strategies;

    public PromotionEngine(IPromocaoRepository promocaoRepository, IEnumerable<IPromotionStrategy> strategies)
    {
        _promocaoRepository = promocaoRepository;
        _strategies = strategies;
    }

    public async Task ProcessarPromocoesAsync(Venda venda)
    {
        if (venda.Itens == null || !venda.Itens.Any())
            return;

        // 1. Busca todas as promoções válidas na data atual
        var promocoesAtivas = await _promocaoRepository.ListarPromocaoAtivaAsync(DateTime.UtcNow);

        // 2. Reseta o subtotal de todos os itens para o valor padrão (preco unitario * qtd) antes do cálculo
        foreach (var item in venda.Itens)
        {
            item.Subtotal = item.Quantidade * item.PrecoUnitario;
        }

        // 3. Varre as promoções ativas e tenta aplicá-las nos itens correspondentes
        foreach (var promocao in promocoesAtivas)
        {
            foreach (var regra in promocao.Regras)
            {
                // Encontra se algum item da venda corresponde ao produto da promoção
                var itemVenda = venda.Itens.FirstOrDefault(i => i.ProdutoId == regra.ProdutoId);
                if (itemVenda == null) continue;

                // Seleciona a estratégia correspondente ao tipo da promoção
                var strategy = _strategies.FirstOrDefault(s => s.Tipo == promocao.Tipo);
                if (strategy != null)
                {
                    strategy.AplicarDesconto(itemVenda, regra);
                }
            }
        }

        // 4. Recalcula o valor final total da venda baseado nos subtotais processados pelo motor
        venda.TotalFinal = venda.Itens.Sum(i => i.Subtotal);
    }
}
