using MongoDB.Bson;
using SharedEntities.Enums;

namespace VideoChecker.Processor.Interfaces;

public interface IQueueRepository
{
    Task<Stream> DownloadVideo(ObjectId jobId);
    Task<bool> InsertEntity<T>(T entity);
    Task<bool> UpdateJobStatus(ObjectId objectId, StatusEnum status, string message);
}
