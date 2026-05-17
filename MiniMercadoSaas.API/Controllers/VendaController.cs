using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Exceptions;
using MiniMercadoSaas.Application.DTO.Request;
using MiniMercadoSaas.Application.DTO.Response;
using MiniMercadoSaas.Application.ServiceInterfaces;
using MiniMercadoSaas.Application.Services;
using MiniMercadoSaas.Domain.Entities;
using MiniMercadoSaas.Domain.Enums;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class VendasController : ControllerBase
{
    private readonly IVendaService _vendaService;

    public VendasController(IVendaService vendaService)
    {
        _vendaService = vendaService;
    }

    [HttpPost]
    public async Task<ActionResult<VendaDetalheResponse>> PostAsync()
    {
        var identity = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(identity) || !Guid.TryParse(identity, out Guid operadorId))
        {
            return Unauthorized("Identificador do operador não encontrado no token.");
        }

        var response = await _vendaService.AbrirAsync(operadorId);

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<VendaDetalheResponse>> GetById(Guid id)
    {
        try
        {
            var response = await _vendaService.GetByIdAsync(id);
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost("{id:guid}/itens")]
    public async Task<IActionResult> AddItem(Guid id, [FromBody] AddItemRequest request)
    {
        try
        {
            var operadorId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var venda = await _vendaService.AddItemAsync(id, request, operadorId);

            var response = new VendaDetalheResponse(venda);

            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (BusinessException ex) when (ex.Message.Contains("aberta"))
        {
            return Conflict(new { message = ex.Message });
        }
        catch (BusinessException ex) when (ex.Message.Contains("Estoque"))
        {
            return UnprocessableEntity(new { message = ex.Message });
        }
        catch (BusinessException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}/itens/{itemId:guid}")]
    public async Task<IActionResult> DeleteItem(Guid id, Guid itemId)
    {
        try
        {
            var venda = await _vendaService.RemoveItemAsync(id, itemId);
            var response = new VendaDetalheResponse(venda);
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (BusinessException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}/finalizar")]
    public async Task<IActionResult> Finalizar(Guid id, [FromBody] FinalizarVendaRequest request)
    {
        try
        {
            var operadorId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var response = await _vendaService.FinalizarAsync(id, request, operadorId);
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (BusinessException ex) when (ex.Message.Contains("Estoque"))
        {
            return UnprocessableEntity(new { message = ex.Message });
        }
        catch (BusinessException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "Gerente,Admin")]
    [HttpPut("{id:guid}/cancelar")]
    public async Task<IActionResult> Cancelar(Guid id, [FromBody] CancelarVendaRequest request)
    {
        try
        {
            var gerenteId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var response = await _vendaService.CancelarAsync(id, request, gerenteId);
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (BusinessException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
