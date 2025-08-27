using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using SharedEntities;

namespace VideoChecker.Domain.Interfaces.ServicesInterfaces;

public interface IVideoCheckerService
{
    Task<ObjectId> UploadVideo(IFormFile video);
    Task<(Stream, string, string)?> DownloadVideo(string id);
    Task<VideoJobStatusChanged?> GetByObjectId(string id);
    Task<List<QrCodeFound>> GetQrCodeFounds(string id);
}
