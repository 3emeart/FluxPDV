using System.Globalization;
using MiniMercadoSaas.Application.DTO.Request;
using MiniMercadoSaas.Application.DTO.Response;
using MiniMercadoSaas.Application.Validators;
using MiniMercadoSaas.Domain;
using MiniMercadoSaas.Domain.Entities;
using FluentValidation;
using MiniMercadoSaas.Application.ServiceInterfaces;
using MiniMercadoSaas.Domain.Interfaces;

namespace MiniMercadoSaas.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly CategoryValidator _categoryValidator;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(ICategoryRepository categoryRepository, CategoryValidator categoryValidator, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _categoryValidator = categoryValidator;
        _unitOfWork = unitOfWork;
    }

    public async Task<CategoriaResponse> CriarCategoria(CategoriaCreateRequest request)
    {
        var validationResult = _categoryValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            var errorMessages = string.Join(" | ", validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            throw new ValidationException(errorMessages);
        }

        var novaCategoria = new Categoria()
        {   
            Nome = request.Nome,
            Descricao = request.Descricao,
            DataCriacao = DateTime.UtcNow,

        };

        await _categoryRepository.AddAsync(novaCategoria);
        await _unitOfWork.CommitAsync();

        return new CategoriaResponse()
        {
            Id = novaCategoria.Id,
            Nome = novaCategoria.Nome,
            Descricao = novaCategoria.Descricao,

        };

    }

    public async Task<CategoriaResponse> BuscarCategoriaPorNome(string nome)
    {
        var categoriaNome = await _categoryRepository.ObterCategoriaPorNome(nome);
        if (categoriaNome == null)
        {
            throw new Exception("Essa categoria não existe");
        }

        return new CategoriaResponse
        {
            Id = categoriaNome.Id,
            Nome = categoriaNome.Nome,
            Descricao = categoriaNome.Descricao,

        };


    }

    public async Task<CategoriaResponse> AtualizarCategoria(int id, CategoriaCreateRequest request)
    {
        var categoriaEditar = await _categoryRepository.ObterCategoriaPorId(id);
        if (categoriaEditar == null)
        {
            throw new KeyNotFoundException("Essa categoria não existe");
        }
        
        categoriaEditar.Id = id;
        categoriaEditar.Nome = request.Nome;
        categoriaEditar.Descricao = request.Descricao;
        categoriaEditar.DataCriacao = DateTime.UtcNow;
        
        await  _categoryRepository.UpdateAsync(categoriaEditar);
        await _unitOfWork.CommitAsync();

        return new CategoriaResponse
        {
            Id = categoriaEditar.Id,
            Nome = categoriaEditar.Nome,
            Descricao = categoriaEditar.Descricao,

        };


    }

    public async Task DeletarCategoria(int id)
    {
        var categoriaExcuir =  _categoryRepository.ObterCategoriaPorId(id);
        if (categoriaExcuir == null)
        {
            throw new Exception("Categoria não existe");
        }
        
        await _categoryRepository.DeleteAsync(id);
        await _unitOfWork.CommitAsync();
        
        
    }

    public async Task<IEnumerable<CategoriaResponse>> ListarCategorias()
    {
        var categorias = await _categoryRepository.ListarCategorias();
        var listaCategorias = categorias.Select(categoria => new CategoriaResponse()
        {
            Id = categoria.Id,
            Nome = categoria.Nome,
            Descricao = categoria.Descricao,

        });
        
        return listaCategorias;
    }
    

    
}