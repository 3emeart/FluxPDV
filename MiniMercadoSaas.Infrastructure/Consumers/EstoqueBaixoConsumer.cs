using MassTransit;
using MiniMercadoSaas.Domain;
using MiniMercadoSaas.Domain.Contracts;

namespace MiniMercadoSaas.Infrastructure.Consumers;

public class EstoqueBaixoConsumer : IConsumer<EstoqueBaixoEvent>
{
    private readonly IEstoqueAlertaNotificador _notificador;

    public EstoqueBaixoConsumer(IEstoqueAlertaNotificador notificador)
    {
        _notificador = notificador;
    }

    public async Task Consume(ConsumeContext<EstoqueBaixoEvent> context)
    {
        var evento =  context.Message;
        
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine($"🚨 ALERTA DE ESTOQUE BAIXO!");
        Console.WriteLine($"Produto: {evento.NomeProduto} (ID: {evento.ProdutoId})");
        Console.WriteLine($"Qtd Atual: {evento.QuantidadeAtual} | Mínimo: {evento.EstoqueMinimo}");
        Console.WriteLine("--------------------------------------------------");

        await _notificador.EnviarAlertaEstoqueBaixoAsync(evento);
    }
}