using Microsoft.AspNetCore.Mvc;

namespace VideoChecker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthCheckController : ControllerBase
{
    [HttpGet]
    public IActionResult HealthCheck() => Ok("Service is running");
}
