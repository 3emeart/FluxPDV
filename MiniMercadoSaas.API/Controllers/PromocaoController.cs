using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniMercadoSaas.Application.DTO.Request;
using MiniMercadoSaas.Application.DTO.Response;
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
    public async Task<IActionResult> Create([FromBody] PromocaoRequest request)
    {
        var promocao = MapearPromocao(request);

        await _promocaoRepository.AddAsync(promocao);
        await _unitOfWork.CommitAsync();

        return CreatedAtAction(nameof(GetById), new { id = promocao.Id }, MapearResponse(promocao));
    }

    [HttpGet("{id}")]
    [AllowAnonymous] // Talvez o frontend precise ver para exibir banners
    public async Task<IActionResult> GetById(Guid id)
    {
        var promocao = await _promocaoRepository.ObterPromocaoPorIdAsync(id);
        if (promocao == null) return NotFound();

        return Ok(MapearResponse(promocao));
    }

    [HttpGet("ativas")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAtivas()
    {
        var promocoes = await _promocaoRepository.ListarPromocaoAtivaAsync(DateTime.UtcNow);
        return Ok(promocoes.Select(MapearResponse));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var promocoes = await _promocaoRepository.GetAllAsync();
        return Ok(promocoes.Select(MapearResponse));
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var promocao = await _promocaoRepository.ObterPromocaoPorIdAsync(id);
        if (promocao == null) return NotFound();

        promocao.Ativo = !promocao.Ativo;
        _promocaoRepository.Update(promocao);
        await _unitOfWork.CommitAsync();

        return Ok(MapearResponse(promocao));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] PromocaoRequest request)
    {
        var promocao = await _promocaoRepository.ObterPromocaoPorIdAsync(id);
        if (promocao == null) return NotFound();

        promocao.Nome = request.Nome;
        promocao.Descricao = request.Descricao;
        promocao.Tipo = request.Tipo;
        promocao.DataInicio = request.DataInicio;
        promocao.DataFim = request.DataFim;
        
        // Simplicando atualização de regras para o momento (remove todas e adiciona as novas)
        promocao.Regras.Clear();
        foreach (var regra in request.Regras)
        {
            promocao.Regras.Add(MapearRegra(promocao.Id, regra));
        }

        _promocaoRepository.Update(promocao);
        await _unitOfWork.CommitAsync();

        return Ok(MapearResponse(promocao));
    }

    private static Promocao MapearPromocao(PromocaoRequest request)
    {
        var promocao = new Promocao(
            request.Nome,
            request.Descricao,
            request.Tipo,
            request.DataInicio,
            request.DataFim
        );

        foreach (var regra in request.Regras)
        {
            promocao.Regras.Add(MapearRegra(promocao.Id, regra));
        }

        return promocao;
    }

    private static RegraPromocao MapearRegra(Guid promocaoId, RegraPromocaoRequest request)
    {
        return new RegraPromocao(
            promocaoId,
            request.ProdutoId,
            request.QuantidadeMinima,
            request.ValorDesconto,
            request.QuantidadePaga
        );
    }

    private static PromocaoResponse MapearResponse(Promocao promocao)
    {
        return new PromocaoResponse
        {
            Id = promocao.Id,
            Nome = promocao.Nome,
            Descricao = promocao.Descricao,
            Tipo = promocao.Tipo,
            DataInicio = promocao.DataInicio,
            DataFim = promocao.DataFim,
            Ativo = promocao.Ativo,
            Regras = promocao.Regras.Select(regra => new RegraPromocaoResponse
            {
                Id = regra.Id,
                PromocaoId = regra.PromocaoId,
                ProdutoId = regra.ProdutoId,
                QuantidadeMinima = regra.QuantidadeMinima,
                ValorDesconto = regra.ValorDesconto,
                QuantidadePaga = regra.QuantidadePaga
            }).ToList()
        };
    }
}
