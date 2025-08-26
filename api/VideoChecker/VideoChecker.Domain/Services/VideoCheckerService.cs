using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using SharedEntities;
using SharedEntities.Enums;
using VideoChecker.Domain.Interfaces.RepositoriesInterfaces;
using VideoChecker.Domain.Interfaces.ServicesInterfaces;
using VideoChecker.Infra.RabbitMQ;

namespace VideoChecker.Domain.Services;

public class VideoCheckerService(
    IVideoCheckerRepository repository,
    IQueueService queueService) : IVideoCheckerService
{
    private readonly IVideoCheckerRepository _repository = repository;
    private readonly IQueueService _queueService = queueService;

    public async Task<ObjectId> UploadVideo(IFormFile video)
    {
        if (video is null || video.Length == 0)
            return ObjectId.Empty;

        using var stream = video.OpenReadStream();
        var objectId = await _repository.SaveVideo(video.FileName, stream);

        var jobChange = new VideoJobStatusChanged(ObjectId.GenerateNewId(), objectId, StatusEnum.Pending, "Registro inserido para processamento", DateTime.UtcNow);
        await _repository.Insert(jobChange);

        var job = new VideoJobCreated(ObjectId.GenerateNewId(), objectId, video.FileName, DateTime.UtcNow);
        await _queueService.Publish("Queue.CreatedJob", job);

        return objectId;
    }

    public async Task<(Stream, string, string)?> DownloadVideo(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId) || objectId == ObjectId.Empty)
            return null;

        return await _repository.GetVideo(objectId);
    }

    public async Task<VideoJobStatusChanged?> GetByObjectId(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId) || objectId == ObjectId.Empty)
            return null;

        return await _repository.GetByObjectId(objectId);
    }
}
