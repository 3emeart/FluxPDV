using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniMercadoSaas.Application.DTO.Response;
using MiniMercadoSaas.Application.ServiceInterfaces;

namespace MiniMercadoSaas.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Gerente,Admin")]
public class FinanceiroController : ControllerBase
{
    private readonly IFinanceiroService _financeiroService;

    public FinanceiroController(IFinanceiroService financeiroService)
    {
        _financeiroService = financeiroService;
    }

    [HttpGet("resumo")]
    public async Task<ActionResult<FinanceiroResumoResponse>> ObterResumo([FromQuery] DateTime? inicio, [FromQuery] DateTime? fim)
    {
        var response = await _financeiroService.ObterResumoAsync(inicio, fim);
        return Ok(response);
    }
}
