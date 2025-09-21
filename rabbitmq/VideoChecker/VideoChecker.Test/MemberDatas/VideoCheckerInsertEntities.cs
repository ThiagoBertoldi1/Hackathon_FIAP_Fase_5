using MongoDB.Bson;
using SharedEntities;

namespace VideoChecker.Test.MemberDatas;

public class VideoCheckerInsertEntities
{
    public static IEnumerable<object[]> GetVideoJobStatusChangedData()
    {
        yield return new object[]
        {
            new QrCodeFound(ObjectId.GenerateNewId(), ObjectId.GenerateNewId(), 10f, "qrcodecontent1", DateTime.Now),
        };

        yield return new object[]
        {
            new QrCodeFound(ObjectId.GenerateNewId(), ObjectId.GenerateNewId(), 10f, "qrcodecontent2", DateTime.Now.AddDays(-1))
        };

        yield return new object[]
        {
            new QrCodeFound(ObjectId.GenerateNewId(), ObjectId.GenerateNewId(), 10f, "qrcodecontent3", DateTime.Now.AddDays(1))
        };
    }
}
