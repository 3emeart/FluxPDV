using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniMercadoSaas.Application.DTO.Request;
using MiniMercadoSaas.Application.ServiceInterfaces;

namespace MiniMercadoSaas.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CategoriaController : ControllerBase
{

    private readonly ICategoryService _categoryService;

    public CategoriaController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Criar(CategoriaCreateRequest request)
    {
        var novaCategoria = await _categoryService.CriarCategoria(request);
        return Created($"api/v1/[controller]/{novaCategoria.Id}", novaCategoria);
    }
    
    [Authorize]
    [HttpGet]

    public async Task<IActionResult> Get()
    {
        var categorias = await _categoryService.ListarCategorias();
        return Ok(categorias);
    }
    
    [Authorize]
    [HttpGet("{nome}")]

    public async Task<IActionResult> GetByName(string nome)
    {
        var categoriaName = await _categoryService.BuscarCategoriaPorNome(nome);
        return Ok(categoriaName);
    }
    
    [Authorize]
    [HttpDelete("{id}")]

    public async Task<IActionResult> Deletar(int id)
    {
        await _categoryService.DeletarCategoria(id);
        return Ok();
    }
    
    [Authorize]
    [HttpPut("{id}")]

    public async Task<IActionResult> Editar(int id,[FromBody] CategoriaCreateRequest request)
    {
        var categorias = await _categoryService.AtualizarCategoria(id, request);
        return Ok(categorias);
        
        
    }






}