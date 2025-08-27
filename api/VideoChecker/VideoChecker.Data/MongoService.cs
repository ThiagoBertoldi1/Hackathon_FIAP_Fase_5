using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace VideoChecker.Data;

public class MongoService(IConfiguration configuration)
{
    private readonly MongoClient _client = new(configuration.GetConnectionString("Bucket"));

    private GridFSBucket? _bucket = null;

    private IMongoDatabase GetBanco()
    {
        return _client.GetDatabase("filesdb");
    }

    protected GridFSBucket GetBucket()
    {
        return _bucket ??= new GridFSBucket(GetBanco());
    }

    protected IMongoCollection<T> GetCollection<T>()
    {
        return GetBanco().GetCollection<T>(typeof(T).Name);
    }

    public async Task<ObjectId> UploadVideo(string name, Stream file)
    {
        var ct = new CancellationTokenSource(TimeSpan.FromSeconds(15)).Token;
        return await GetBucket().UploadFromStreamAsync(name, file, cancellationToken: ct);
    }
}