using MongoDB.Bson;
using Moq;
using SharedEntities;
using VideoChecker.Processor.Interfaces;
using VideoChecker.Test.MemberDatas;

namespace VideoChecker.Test.VideoChecker;

public class VideoCheckerTests
{
    private readonly Mock<IQueueRepository> _queueRepository = new();

    [Theory]
    [InlineData(1024)]
    [InlineData(10 * 1024)]
    [InlineData(10 * 1024 * 1024)]
    public async Task VideoCheckerDownloadVideo_ShouldReturnVideoStream(int tamanhoArray)
    {
        var objectId = ObjectId.GenerateNewId();

        var array = new byte[tamanhoArray];
        new Random().NextBytes(array);
        using var stream = new MemoryStream(array);

        var setup = _queueRepository.Setup(x => x.DownloadVideo(objectId))
            .ReturnsAsync(stream);

        var result = await _queueRepository.Object.DownloadVideo(objectId);

        Assert.NotNull(result);
        Assert.Equal(tamanhoArray, result.Length);
    }

    [Theory]
    [MemberData(nameof(VideoCheckerInsertEntities.GetVideoJobStatusChangedData), MemberType = typeof(VideoCheckerInsertEntities))]
    public async Task VideoCheckerInsertEntity_ShouldInsertEntity(QrCodeFound entity)
    {
        var setup = _queueRepository.Setup(x => x.InsertEntity(It.IsAny<object>()))
            .ReturnsAsync(true);

        var result = await _queueRepository.Object.InsertEntity(entity);

        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(VideoCheckerUpdateEntities.GetVideoJobStatusChangedData), MemberType = typeof(VideoCheckerUpdateEntities))]
    public async Task VideoCheckerUpdateJobStatus_ShouldUpdateJobStatus(VideoJobStatusChanged entity)
    {
        var setup = _queueRepository.Setup(x => x.UpdateJobStatus(entity.JobId, entity.Status, entity.Message ?? string.Empty))
            .ReturnsAsync(true);

        var result = await _queueRepository.Object.UpdateJobStatus(entity.JobId, entity.Status, entity.Message ?? string.Empty);

        Assert.True(result);
    }
}
