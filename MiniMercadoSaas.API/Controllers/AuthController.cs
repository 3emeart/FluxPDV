using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MiniMercadoSaas.Application.DTO.Response;
using MiniMercadoSaas.Application.ServiceInterfaces;
using LoginRequest = MiniMercadoSaas.Application.DTO.Request.LoginRequest;

namespace MiniMercadoSaas.API.Controllers;

[ApiController ]
[Route("api/auth/login")]

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [AllowAnonymous]
    [HttpPost]
    
    

    public async Task<ActionResult<LoginResponse>> Login ([FromBody] LoginRequest  request)
    {
        try
        {
            var login = await _authService.LoginAsync(request);
            return Ok(login);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

}