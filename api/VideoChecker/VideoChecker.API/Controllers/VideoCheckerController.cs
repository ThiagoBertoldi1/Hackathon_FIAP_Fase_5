using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using VideoChecker.Domain.Interfaces.ServicesInterfaces;

namespace VideoChecker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VideoCheckerController(IVideoCheckerService videoCheckerService) : ControllerBase
{
    private readonly IVideoCheckerService _videoCheckerService = videoCheckerService;

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile video)
    {
        var response = await _videoCheckerService.UploadVideo(video);

        return response != ObjectId.Empty
            ? Ok($"Video uploaded successfully -> {response}")
            : BadRequest("Upload failed");
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetAll([FromQuery] string id)
    {
        var (file, type, name) = (await _videoCheckerService.DownloadVideo(id)).Value;

        return file is null || file.Length == 0
            ? NotFound("Video not found")
            : File(file, type, name);
    }
}