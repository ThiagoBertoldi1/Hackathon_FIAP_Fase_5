using MongoDB.Bson;
using VideoChecker.Domain.Interfaces.RepositoriesInterfaces;

namespace VideoChecker.Data.Repository;

public class VideoCheckerRepository(IMongoService service) : IVideoCheckerRepository
{
    private readonly IMongoService _service = service;

    public async Task<ObjectId> SaveVideo(string name, Stream video)
    {
        return await _service.UploadVideo(name, video);
    }

    public async Task<(Stream, string, string)?> GetVideo(ObjectId objectId)
    {
        return await _service.DownloadVideo(objectId);
    }
}