namespace MiniMercadoSaas.Domain.Contracts;

public record EstoqueBaixoEvent(
    int ProdutoId,
    string NomeProduto,
    int QuantidadeAtual,
    int EstoqueMinimo
        );