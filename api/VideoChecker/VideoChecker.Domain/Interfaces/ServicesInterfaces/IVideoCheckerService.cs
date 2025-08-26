using Microsoft.AspNetCore.Http;
using MongoDB.Bson;

namespace VideoChecker.Domain.Interfaces.ServicesInterfaces;

public interface IVideoCheckerService
{
    Task<ObjectId> UploadVideo(IFormFile video);
    Task<(Stream, string, string)?> DownloadVideo(ObjectId objectId);
}
