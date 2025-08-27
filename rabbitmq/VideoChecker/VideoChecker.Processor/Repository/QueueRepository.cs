using MongoDB.Bson;
using MongoDB.Driver;
using SharedEntities;
using SharedEntities.Enums;
using VideoChecker.Processor.Interfaces;
using VideoChecker.Processor.Repository.Base;

namespace VideoChecker.Processor.Repository;

public class QueueRepository(IConfiguration configuration) : MongoBase(configuration), IQueueRepository
{
    public async Task<Stream> DownloadVideo(ObjectId jobId)
    {
        return await GetBucket().OpenDownloadStreamAsync(jobId);
    }

    public async Task<bool> InsertEntity<T>(T entity)
    {
        try
        {
            var collection = GetCollection<T>();
            await collection.InsertOneAsync(entity);
            return true;
        }
        catch { return false; }
    }

    public async Task<bool> UpdateJobStatus(ObjectId objectId, StatusEnum status, string message)
    {
        var collection = GetCollection<VideoJobStatusChanged>();

        var filter = Builders<VideoJobStatusChanged>.Filter
            .Eq(u => u.JobId, objectId);

        var update = Builders<VideoJobStatusChanged>.Update
            .Set(u => u.Status, status)
            .Set(x => x.Message, message);

        var result = await collection.UpdateOneAsync(filter, update);

        return result.ModifiedCount > 0;
    }
}
