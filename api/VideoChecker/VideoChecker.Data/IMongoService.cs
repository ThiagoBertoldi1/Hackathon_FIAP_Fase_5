using MongoDB.Bson;

namespace VideoChecker.Data;

public interface IMongoService
{
    Task<ObjectId> UploadVideo(string name, Stream file);
    Task<(Stream, string, string)?> DownloadVideo(ObjectId objectId);
}
