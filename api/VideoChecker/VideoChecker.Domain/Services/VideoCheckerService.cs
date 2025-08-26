using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using VideoChecker.Domain.Interfaces.RepositoriesInterfaces;
using VideoChecker.Domain.Interfaces.ServicesInterfaces;

namespace VideoChecker.Domain.Services;

public class VideoCheckerService(IVideoCheckerRepository repository) : IVideoCheckerService
{
    private readonly IVideoCheckerRepository _repository = repository;

    public async Task<ObjectId> UploadVideo(IFormFile video)
    {
        if (video is null || video.Length == 0)
            return ObjectId.Empty;

        using var stream = video.OpenReadStream();
        return await _repository.SaveVideo(video.Name, stream);
    }

    public async Task<(Stream, string, string)?> DownloadVideo(ObjectId objectId)
    {
        return await _repository.GetVideo(objectId);
    }
}
