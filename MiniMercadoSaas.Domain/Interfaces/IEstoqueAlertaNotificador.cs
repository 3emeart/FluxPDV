using MiniMercadoSaas.Domain.Contracts;

namespace MiniMercadoSaas.Domain;

public interface IEstoqueAlertaNotificador
{
    Task EnviarAlertaEstoqueBaixoAsync(EstoqueBaixoEvent evento);
}
