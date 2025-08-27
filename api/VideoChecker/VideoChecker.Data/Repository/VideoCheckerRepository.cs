using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using SharedEntities;
using VideoChecker.Domain.Interfaces.RepositoriesInterfaces;

namespace VideoChecker.Data.Repository;

public class VideoCheckerRepository(IConfiguration configuration) : MongoService(configuration), IVideoCheckerRepository
{
    public async Task<(Stream, string, string)?> GetVideo(ObjectId objectId)
    {
        var stream = await GetBucket().OpenDownloadStreamAsync(objectId);

        if (stream.Length == 0)
            return null;

        var info = stream.FileInfo;

        var contentType = info.Metadata?.GetValue("contentType", null)?.AsString;
        if (string.IsNullOrWhiteSpace(contentType))
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(info.Filename, out contentType))
                contentType = "application/octet-stream";
        }

        return (stream, contentType, info.Filename);
    }

    public async Task Insert<T>(T data)
    {
        var collection = GetCollection<T>();
        await collection.InsertOneAsync(data);
    }

    public async Task<VideoJobStatusChanged?> GetByObjectId(ObjectId objectId)
    {
        return await GetCollection<VideoJobStatusChanged>().Find(x => x.JobId == objectId).FirstOrDefaultAsync();
    }

    public async Task<ObjectId> SaveVideo(string name, Stream file)
    {
        return await UploadVideo(name, file);
    }

    public Task<List<QrCodeFound>> GetQrCodeFounds(ObjectId objectId)
    {
        return GetCollection<QrCodeFound>().Find(x => x.JobId == objectId).ToListAsync();
    }
}