using Exceptions;
using MiniMercadoSaas.Application.DTO.Request;
using MiniMercadoSaas.Application.DTO.Response;
using MiniMercadoSaas.Application.ServiceInterfaces;
using MiniMercadoSaas.Domain;
using MiniMercadoSaas.Domain.Entities;
using MiniMercadoSaas.Domain.Enums;
using MiniMercadoSaas.Domain.Interfaces;

namespace MiniMercadoSaas.Application.Services;

public class EstoqueService : IEstoqueService
{
    private readonly IProductRepository _productRepository;
    private readonly IMovimentacaoEstoqueRepository _movimentacaoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EstoqueService(IProductRepository productRepository, IMovimentacaoEstoqueRepository movimentacaoRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _movimentacaoRepository = movimentacaoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<MovimentacaoEstoqueResponse> RegistrarEntradaAsync(EntradaEstoqueRequest request, Guid usuarioId)
    {
        if (request.Quantidade <= 0)
            throw new BusinessException("A quantidade deve ser maior que zero");

        var produto = await _productRepository.FindByIdAsync(request.ProdutoId)
                      ?? throw new NotFoundException("Produto não encontrado");

        produto.Quantidade += request.Quantidade;
        await _productRepository.UpdateAsync(produto);

        var movimentacao = new MovimentacaoEstoque
        {
            ProdutoId = produto.Id,
            Quantidade = request.Quantidade,
            Tipo = TipoMovimentacao.EntradaManual,
            UsuarioId = usuarioId,
            Observacao = request.Observacao
        };

        await _movimentacaoRepository.AddAsync(movimentacao);
        await _unitOfWork.CommitAsync();

        return new MovimentacaoEstoqueResponse
        {
            Id = movimentacao.Id,
            ProdutoId = produto.Id,
            NomeProduto = produto.Nome,
            Quantidade = movimentacao.Quantidade,
            Tipo = movimentacao.Tipo,
            Observacao = movimentacao.Observacao,
            CriadoEm = movimentacao.CriadoEm
        };
    }

    public async Task<IEnumerable<MovimentacaoEstoqueResponse>> ConsultarMovimentacoesAsync(int produtoId)
    {
        var movimentacoes = await _movimentacaoRepository.GetByProdutoIdAsync(produtoId);

        return movimentacoes.Select(m => new MovimentacaoEstoqueResponse
        {
            Id = m.Id,
            ProdutoId = m.ProdutoId,
            NomeProduto = m.Produto.Nome,
            Quantidade = m.Quantidade,
            Tipo = m.Tipo,
            VendaId = m.VendaId,
            Observacao = m.Observacao,
            CriadoEm = m.CriadoEm
        });
    }

    public async Task<IEnumerable<EstoqueResumoResponse>> ConsultarEstoqueBaixoAsync()
    {
        var produtos = await _productRepository.ListAsync();

        return produtos
            .Where(p => p.Ativo && p.Quantidade <= p.EstoqueMinimo)
            .Select(p => new EstoqueResumoResponse
            {
                ProdutoId = p.Id,
                NomeProduto = p.Nome,
                Codigo = p.Codigo,
                QuantidadeAtual = p.Quantidade,
                EstoqueMinimo = p.EstoqueMinimo,
                AbaixoDoMinimo = p.Quantidade <= p.EstoqueMinimo
            });
    }
}
