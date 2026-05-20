using Microsoft.AspNetCore.SignalR;
using MiniMercadoSaas.API.Hubs;
using MiniMercadoSaas.Domain;
using MiniMercadoSaas.Domain.Contracts;

namespace MiniMercadoSaas.API.Services;

public class SignalREstoqueAlertaNotificador : IEstoqueAlertaNotificador
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public SignalREstoqueAlertaNotificador(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task EnviarAlertaEstoqueBaixoAsync(EstoqueBaixoEvent evento)
    {
        await _hubContext.Clients.All.SendAsync("ReceberAlertaEstoqueBaixo", new
        {
            evento.ProdutoId,
            evento.NomeProduto,
            evento.QuantidadeAtual,
            evento.EstoqueMinimo,
            Mensagem = $"🚨 O produto '{evento.NomeProduto}' atingiu o estoque mínimo de segurança ({evento.QuantidadeAtual} restantes)."
        });
    }
}
