using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniMercadoSaas.Domain.Entities;
using MiniMercadoSaas.Domain.Interfaces;

namespace MiniMercadoSaas.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
//[Authorize(Roles = "Admin,Gerente")]
public class PromocaoController : ControllerBase
{
    private readonly IPromocaoRepository _promocaoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PromocaoController(IPromocaoRepository promocaoRepository, IUnitOfWork unitOfWork)
    {
        _promocaoRepository = promocaoRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Promocao promocao)
    {
        // Nota: Para um projeto completo, use DTOs (Data Transfer Objects) e FluentValidation aqui.
        // Como este é um módulo novo, permitiremos a entidade direta para fins de teste inicial.
        await _promocaoRepository.AddAsync(promocao);
        await _unitOfWork.CommitAsync();

        return CreatedAtAction(nameof(GetById), new { id = promocao.Id }, promocao);
    }

    [HttpGet("{id}")]
    [AllowAnonymous] // Talvez o frontend precise ver para exibir banners
    public async Task<IActionResult> GetById(Guid id)
    {
        var promocao = await _promocaoRepository.ObterPromocaoPorIdAsync(id);
        if (promocao == null) return NotFound();

        return Ok(promocao);
    }

    [HttpGet("ativas")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAtivas()
    {
        var promocoes = await _promocaoRepository.ListarPromocaoAtivaAsync(DateTime.UtcNow);
        return Ok(promocoes);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var promocoes = await _promocaoRepository.GetAllAsync();
        return Ok(promocoes);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var promocao = await _promocaoRepository.ObterPromocaoPorIdAsync(id);
        if (promocao == null) return NotFound();

        promocao.Ativo = !promocao.Ativo;
        _promocaoRepository.Update(promocao);
        await _unitOfWork.CommitAsync();

        return Ok(promocao);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Promocao promocaoAtualizada)
    {
        var promocao = await _promocaoRepository.ObterPromocaoPorIdAsync(id);
        if (promocao == null) return NotFound();

        promocao.Nome = promocaoAtualizada.Nome;
        promocao.Descricao = promocaoAtualizada.Descricao;
        promocao.Tipo = promocaoAtualizada.Tipo;
        promocao.DataInicio = promocaoAtualizada.DataInicio;
        promocao.DataFim = promocaoAtualizada.DataFim;
        
        // Simplicando atualização de regras para o momento (remove todas e adiciona as novas)
        promocao.Regras.Clear();
        foreach (var regra in promocaoAtualizada.Regras)
        {
            promocao.Regras.Add(regra);
        }

        _promocaoRepository.Update(promocao);
        await _unitOfWork.CommitAsync();

        return Ok(promocao);
    }
}
