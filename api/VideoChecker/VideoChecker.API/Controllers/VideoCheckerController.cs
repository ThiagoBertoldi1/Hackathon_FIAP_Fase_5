using Microsoft.AspNetCore.Mvc;

namespace VideoChecker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VideoCheckerController : ControllerBase
{
    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile video)
    {
        await Task.Delay(1000);
        return Ok("Video uploaded successfully");
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetAll()
    {
        await Task.Delay(1000);
        return Ok("Lista dos processamentos");
    }
}