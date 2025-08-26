using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace VideoChecker.Data;

internal class MongoService(IConfiguration configuration) : IMongoService
{
    private readonly MongoClient _client = new(configuration.GetConnectionString("DB"));

    private GridFSBucket? _bucket = null;

    private GridFSBucket GetBucket()
    {
        if (_bucket is null)
        {
            var db = _client.GetDatabase("filesdb");
            _bucket = new GridFSBucket(db);
        }

        return _bucket;
    }

    public async Task<ObjectId> UploadVideo(string name, Stream file)
    {
        var ct = new CancellationTokenSource(TimeSpan.FromSeconds(15)).Token;
        return await GetBucket().UploadFromStreamAsync(name, file/*, cancellationToken: ct*/);
    }

    public async Task<(Stream, string, string)?> DownloadVideo(ObjectId objectId)
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
}