using MiniMercadoSaas.Application.DTO.Response;
using MiniMercadoSaas.Application.ServiceInterfaces;
using MiniMercadoSaas.Domain.Entities;
using MiniMercadoSaas.Domain.Enums;
using MiniMercadoSaas.Domain.Interfaces;

namespace MiniMercadoSaas.Application.Services;

public class FinanceiroService : IFinanceiroService
{
    private readonly IVendaRepository _vendaRepository;

    public FinanceiroService(IVendaRepository vendaRepository)
    {
        _vendaRepository = vendaRepository;
    }

    public async Task<FinanceiroResumoResponse> ObterResumoAsync(DateTime? inicio, DateTime? fim)
    {
        var dataFim = (fim ?? DateTime.Today).Date.AddDays(1).AddTicks(-1);
        var dataInicio = (inicio ?? DateTime.Today.AddDays(-6)).Date;

        if (dataInicio > dataFim)
        {
            (dataInicio, dataFim) = (dataFim.Date, dataInicio.Date.AddDays(1).AddTicks(-1));
        }

        var vendas = (await _vendaRepository.ListarPorPeriodoAsync(dataInicio, dataFim, true)).ToList();
        var vendasFinalizadas = vendas.Where(v => v.Status == StatusVenda.Finalizado).ToList();
        var vendasCanceladas = vendas.Where(v => v.Status == StatusVenda.Cancelado).ToList();

        var faturamentoBruto = vendasFinalizadas.Sum(v => v.TotalFinal);
        var vendasFinalizadasCount = vendasFinalizadas.Count;

        return new FinanceiroResumoResponse
        {
            Inicio = dataInicio,
            Fim = dataFim,
            FaturamentoBruto = faturamentoBruto,
            FaturamentoCancelado = vendasCanceladas.Sum(v => v.TotalFinal),
            TicketMedio = vendasFinalizadasCount == 0 ? 0 : faturamentoBruto / vendasFinalizadasCount,
            VendasFinalizadas = vendasFinalizadasCount,
            VendasCanceladas = vendasCanceladas.Count,
            VendasAbertas = vendas.Count(v => v.Status == StatusVenda.Aberta),
            ItensVendidos = vendasFinalizadas.Sum(v => v.Itens?.Sum(i => i.Quantidade) ?? 0),
            FormasPagamento = MontarFormasPagamento(vendasFinalizadas),
            SerieDiaria = MontarSerieDiaria(vendasFinalizadas, dataInicio, dataFim),
            UltimasVendas = vendas
                .OrderByDescending(DataReferencia)
                .Take(20)
                .Select(ToVendaResponse)
                .ToList()
        };
    }

    private static IEnumerable<FinanceiroFormaPagamentoResponse> MontarFormasPagamento(IEnumerable<Venda> vendas)
    {
        return vendas
            .GroupBy(v => v.FormaPagamento)
            .Select(g => new FinanceiroFormaPagamentoResponse
            {
                FormaPagamento = (int)g.Key,
                Nome = NomeFormaPagamento(g.Key),
                Total = g.Sum(v => v.TotalFinal),
                Quantidade = g.Count()
            })
            .OrderByDescending(item => item.Total)
            .ToList();
    }

    private static IEnumerable<FinanceiroSerieDiaResponse> MontarSerieDiaria(IEnumerable<Venda> vendas, DateTime inicio, DateTime fim)
    {
        var vendasPorDia = vendas
            .GroupBy(v => (v.FinalizadaEm ?? v.AbertaEm).Date)
            .ToDictionary(g => g.Key, g => new
            {
                Total = g.Sum(v => v.TotalFinal),
                Vendas = g.Count()
            });

        var dias = new List<FinanceiroSerieDiaResponse>();
        for (var data = inicio.Date; data <= fim.Date; data = data.AddDays(1))
        {
            vendasPorDia.TryGetValue(data, out var dia);
            dias.Add(new FinanceiroSerieDiaResponse
            {
                Data = data,
                Total = dia?.Total ?? 0,
                Vendas = dia?.Vendas ?? 0
            });
        }

        return dias;
    }

    private static FinanceiroVendaResponse ToVendaResponse(Venda venda)
    {
        return new FinanceiroVendaResponse
        {
            Id = venda.Id,
            Status = (int)venda.Status,
            FormaPagamento = (int)venda.FormaPagamento,
            TotalFinal = venda.TotalFinal,
            AbertaEm = venda.AbertaEm,
            FinalizadaEm = venda.FinalizadaEm,
            CanceladaEm = venda.CanceladaEm,
            Itens = venda.Itens?.Sum(i => i.Quantidade) ?? 0
        };
    }

    private static DateTime DataReferencia(Venda venda)
    {
        return venda.FinalizadaEm ?? venda.CanceladaEm ?? venda.AbertaEm;
    }

    private static string NomeFormaPagamento(FormaPagamento formaPagamento)
    {
        return formaPagamento switch
        {
            FormaPagamento.Dinheiro => "Dinheiro",
            FormaPagamento.Cartao => "Cartão",
            FormaPagamento.Pix => "Pix",
            _ => "Outro"
        };
    }
}
