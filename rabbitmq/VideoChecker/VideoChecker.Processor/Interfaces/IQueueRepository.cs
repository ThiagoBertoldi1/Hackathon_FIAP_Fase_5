using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using SharedEntities.Enums;

namespace VideoChecker.Processor.Interfaces;

public interface IQueueRepository
{
    Task<bool> SaveVideo(IFormFile video);
    Task<bool> InsertEntity<T>(T entity);
    Task<bool> UpdateJobStatus(ObjectId objectId, StatusEnum status, string message);
}
