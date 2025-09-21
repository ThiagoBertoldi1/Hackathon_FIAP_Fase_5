using MongoDB.Bson;
using SharedEntities;
using SharedEntities.Enums;

namespace VideoChecker.Test.MemberDatas;

public class VideoCheckerUpdateEntities
{
    public static IEnumerable<object[]> GetVideoJobStatusChangedData()
    {
        yield return new object[]
        {
            new VideoJobStatusChanged(ObjectId.GenerateNewId(), ObjectId.GenerateNewId(), StatusEnum.Pending, null, "name", DateTime.Now),
        };

        yield return new object[]
        {
            new VideoJobStatusChanged(ObjectId.GenerateNewId(), ObjectId.GenerateNewId(), StatusEnum.Pending, null, "name", DateTime.Now.AddDays(-1))
        };

        yield return new object[]
        {
            new VideoJobStatusChanged(ObjectId.GenerateNewId(), ObjectId.GenerateNewId(), StatusEnum.Pending, null, "name", DateTime.Now.AddDays(1))
        };
    }
}
