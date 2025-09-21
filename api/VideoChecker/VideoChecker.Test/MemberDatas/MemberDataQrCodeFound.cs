using MongoDB.Bson;
using SharedEntities;

namespace VideoChecker.Test.MemberDatas;
public class MemberDataQrCodeFound
{
    public static IEnumerable<object[]> GetVideoJobStatusChangedData()
    {
        yield return new object[]
        {
            ObjectId.GenerateNewId(),
            new List<QrCodeFound>
            {
                new(
                    ObjectId.GenerateNewId(),
                    ObjectId.GenerateNewId(),
                    10f,
                    "QRCodeContent1",
                    DateTime.UtcNow
                    ),
                new(
                    ObjectId.GenerateNewId(),
                    ObjectId.GenerateNewId(),
                    10f,
                    "QRCodeContent2",
                    DateTime.UtcNow
                    ),
                new(
                    ObjectId.GenerateNewId(),
                    ObjectId.GenerateNewId(),
                    10f,
                    "QRCodeContent3",
                    DateTime.UtcNow
                    ),
            }
        };
    }
}
