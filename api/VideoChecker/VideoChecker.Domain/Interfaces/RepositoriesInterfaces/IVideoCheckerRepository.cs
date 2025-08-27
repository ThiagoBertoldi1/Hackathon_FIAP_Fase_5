using MongoDB.Bson;
using SharedEntities;

namespace VideoChecker.Domain.Interfaces.RepositoriesInterfaces;

public interface IVideoCheckerRepository
{
    Task<ObjectId> SaveVideo(string name, Stream file);
    Task<(Stream, string, string)?> GetVideo(ObjectId objectId);
    Task Insert<T>(T data);
    Task<VideoJobStatusChanged?> GetByObjectId(ObjectId objectId);
}
