using Microsoft.AspNetCore.Mvc;
using MiniMercadoSaas.Application.DTO.Request;
using MiniMercadoSaas.Application.ServiceInterfaces;
using MiniMercadoSaas.Domain.Entities;

namespace MiniMercadoSaas.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    
    public  UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateRequest request)
    {
        var novoUsuario = await _userService.Create(request);
        return Ok(ToResponse(novoUsuario));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users.Select(ToResponse));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _userService.DeleteAsync(id);
        return NoContent();
    }

    private static object ToResponse(User user)
    {
        return new
        {
            user.Id,
            user.Name,
            user.Email,
            user.Active,
            user.Role,
            user.CreatedIn,
            user.UltimoLoginIn
        };
    }
}
