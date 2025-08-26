using MongoDB.Bson;

namespace VideoChecker.Domain.Interfaces.RepositoriesInterfaces;

public interface IVideoCheckerRepository
{
    Task<ObjectId> SaveVideo(string name, Stream video);
    Task<(Stream, string, string)?> GetVideo(ObjectId objectId);
}
