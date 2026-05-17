using Microsoft.AspNetCore.Mvc;
using MiniMercadoSaas.Domain.Entities;
using MiniMercadoSaas.Infrastructure.Context;

namespace MiniMercadoSaas.API.Controllers;

[ApiController]
[Route("[controller]")]

public class HealthCheckController : ControllerBase
{
   

    [HttpGet]

    public  OkObjectResult GetHealthCheck()
    {
        return Ok($"Healtly {DateTime.Now}");
    }
}