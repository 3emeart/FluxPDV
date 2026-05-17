using FluentValidation;
using MiniMercadoSaas.Application.DTO.Request;
using MiniMercadoSaas.Application.DTO.Response;
using MiniMercadoSaas.Application.ServiceInterfaces;
using MiniMercadoSaas.Domain;
using MiniMercadoSaas.Domain.Entities;
using MiniMercadoSaas.Domain.Interfaces;

namespace MiniMercadoSaas.Application.Services;

public class ProdutoService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator<ProdutoRequest> _requestValidator;
    private readonly IUnitOfWork _unitOfWork;

    public ProdutoService(IProductRepository productRepository, IValidator<ProdutoRequest> validator, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _requestValidator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ProdutoResponse>> BuscarTodos()
    {
        var listaProdutos = await _productRepository.ListAsync();

        var produtosLista = listaProdutos.Select(produto => new ProdutoResponse()
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Codigo = produto.Codigo,
            PrecoCompra = produto.PrecoCompra,
            PrecoVenda = produto.PrecoVenda,
            QuantidadeAtual = produto.Quantidade,
        });
      
            return produtosLista;
 
    }

    public async Task<ProdutoResponse> BuscarPorId(int id)
    {
        var produtoPorId = await _productRepository.FindByIdAsync(id);
        if (produtoPorId == null)
        {
            throw new Exception($"Produto {id} not found");
        }

        return new ProdutoResponse
        {
            Id = produtoPorId.Id,
            Nome = produtoPorId.Nome,
            Codigo = produtoPorId.Codigo,
            PrecoCompra = produtoPorId.PrecoCompra,
            PrecoVenda = produtoPorId.PrecoVenda,
            QuantidadeAtual = produtoPorId.Quantidade,
        };

    }

    public async Task<ProdutoResponse> Create(ProdutoRequest request)
    {
        var validationResult = _requestValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            var errorMessages = string.Join(" | ", validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            throw new Exception(errorMessages);
        }

        var novoProduto = new Produto
        {
            Nome = request.Nome,
            Codigo = request.Codigo,
            PrecoCompra = request.PrecoCompra,
            PrecoVenda = request.PrecoVenda,
            Quantidade = request.QuantidadeInicial,
            CategoriaId = request.CategoriaId,
            Categoria = null,



        };

        await _productRepository.AddAsync(novoProduto);
        await _unitOfWork.CommitAsync();

        return new ProdutoResponse
        {
            Id = novoProduto.Id,
            Nome = novoProduto.Nome,
            Codigo = novoProduto.Codigo,
            PrecoCompra = novoProduto.PrecoCompra,
            PrecoVenda = novoProduto.PrecoVenda,
            QuantidadeAtual = novoProduto.Quantidade,
            
        };


    }

    public async Task<ProdutoResponse> Update(int id, ProdutoRequest request)
    {
        var validationResult =  _requestValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(" | ", validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            throw new Exception(errors);
        }
        
        var produtoIsNull = await _productRepository.FindByIdAsync(id);
        if (produtoIsNull == null)
        {
            throw new Exception($"Produto {id} not found");
        }
        
        var produtoCodigo = _productRepository.FindByCodigo(request.Codigo);
        if (produtoCodigo != null && produtoCodigo.Id != id)
        {
            throw new Exception("O código de barras informado já pertence a outro produto no banco de dados");
        }

        produtoIsNull.Nome = request.Nome;
        produtoIsNull.PrecoVenda = request.PrecoVenda;
        produtoIsNull.PrecoCompra = request.PrecoCompra;
        produtoIsNull.Codigo = request.Codigo;
        
        await _productRepository.UpdateAsync(produtoIsNull);
        await _unitOfWork.CommitAsync();

        return new ProdutoResponse
        {
            Id = produtoIsNull.Id,
            Nome = produtoIsNull.Nome,
            Codigo = produtoIsNull.Codigo,
            PrecoCompra = produtoIsNull.PrecoCompra,
            PrecoVenda = produtoIsNull.PrecoVenda,
            QuantidadeAtual = produtoIsNull.Quantidade,
        };

    }

    public async Task Delete(int id)
    {
        var produto = await _productRepository.FindByIdAsync(id);
        if (produto == null)
        {
            throw new Exception("Produto not found");
        }

        /*
        if (produto.Quantidade > 0)
        {
            throw new Exception("Não é permitido deletar um produto que ainda possui unidades no estoque");
        }
        */

        await _productRepository.DeleteAsync(id);
        await _unitOfWork.CommitAsync();

    }

    





}