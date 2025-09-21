using MongoDB.Bson;
using SharedEntities;
using SharedEntities.Enums;

namespace VideoChecker.Test.MemberDatas;

public class MemberDataGetAll
{
    public static IEnumerable<object[]> GetVideoJobStatusChangedData()
    {
        yield return new object[]
        {
            new List<VideoJobStatusChanged>
            {
                new(
                    ObjectId.GenerateNewId(),
                    ObjectId.GenerateNewId(),
                    StatusEnum.Pending,
                    "Aguardando processamento",
                    "arquivo1.mp4",
                    DateTime.UtcNow),
                new(
                    ObjectId.GenerateNewId(),
                    ObjectId.GenerateNewId(),
                    StatusEnum.Completed,
                    "Aguardando processamento",
                    "arquivo1.mp4",
                    DateTime.UtcNow),
                new(
                    ObjectId.GenerateNewId(),
                    ObjectId.GenerateNewId(),
                    StatusEnum.Processing,
                    "Aguardando processamento",
                    "arquivo1.mp4",
                    DateTime.UtcNow),
                new(
                    ObjectId.GenerateNewId(),
                    ObjectId.GenerateNewId(),
                    StatusEnum.Failed,
                    "Aguardando processamento",
                    "arquivo1.mp4",
                    DateTime.UtcNow)
            }
        };
    }
}
