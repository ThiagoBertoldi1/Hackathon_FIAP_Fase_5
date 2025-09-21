using MongoDB.Bson;
using Moq;
using SharedEntities;
using SharedEntities.Enums;
using VideoChecker.Domain.Interfaces.RepositoriesInterfaces;
using VideoChecker.Domain.Interfaces.ServicesInterfaces;
using VideoChecker.Test.Helper;
using VideoChecker.Test.MemberDatas;

namespace VideoChecker.Test.VideoChecker;

public class VideoCheckerTests
{
    private readonly Mock<IVideoCheckerRepository> _repo = new();
    private readonly Mock<IVideoCheckerService> _service = new();

    [Theory]
    [InlineData(1024)]
    [InlineData(5 * 1024)]
    [InlineData(5 * 1024 * 1024)]
    public async Task VideoCheckerUpload_ShouldBeValidUpload(int fileSize)
    {
        var fakeVideo = new byte[fileSize];
        new Random().NextBytes(fakeVideo);
        using var stream = new MemoryStream(fakeVideo);

        var setup = _repo.Setup(r => r.SaveVideo(It.IsAny<string>(), It.IsAny<Stream>()))
            .ReturnsAsync(ObjectId.GenerateNewId());

        var objectId = await _repo.Object.SaveVideo("", stream);

        Assert.NotEqual(ObjectId.Empty, objectId);
    }

    [Theory]
    [InlineData("64f7c8e2a1b2c3d4e5f60789", 1024, "video.mp4", "application/octet-stream")]
    [InlineData("507f1f77bcf86cd799439011", 10 * 1024, "video2.mp4", "application/octet-stream")]
    [InlineData("60d5db2f8e4b0c7d2a3f1e99", 10 * 1024 * 1024, "video3.mp4", "application/octet-stream")]
    public async Task VideoCheckerGetVideo_ShouldReturnValidVideo(string stringObjectId, int arrayByteCount, string fileName, string contentType)
    {
        var objectId = ObjectId.Parse(stringObjectId);

        var arrayByte = new byte[arrayByteCount];
        new Random().NextBytes(arrayByte);
        using var stream = new MemoryStream(arrayByte);

        var setup = _repo.Setup(r => r.GetVideo(It.IsAny<ObjectId>()))
            .ReturnsAsync((stream, contentType, fileName));

        var result = await _repo.Object.GetVideo(objectId);

        Assert.NotNull(result);
        Assert.Equal(result?.Item1.Length, arrayByteCount);
        Assert.Equal(contentType, result?.Item2);
        Assert.Equal(fileName, result?.Item3);
    }

    [Theory]
    [InlineData(StatusEnum.Pending, "Pendente", "arquivo1.mp4")]
    [InlineData(StatusEnum.Processing, "Pendente", "arquivo1.mp4")]
    [InlineData(StatusEnum.Completed, "Pendente", "arquivo1.mp4")]
    [InlineData(StatusEnum.Failed, "Pendente", "arquivo1.mp4")]
    public async Task VideoCheckerInsertJobStatus_ShouldInsertValidJob(StatusEnum status, string message, string fileName)
    {
        var id = ObjectId.GenerateNewId();
        var jobId = ObjectId.GenerateNewId();

        var objeto = new VideoJobStatusChanged(id, jobId, status, message, fileName, DateTime.Now);

        var setup = _repo.Setup(r => r.Insert(It.IsAny<object>()))
            .Returns(Task.CompletedTask);

        var exception = await Record.ExceptionAsync(() => _repo.Object.Insert(objeto));

        Assert.Null(exception);
    }

    [Theory]
    [InlineData("64f7c8e2a1b2c3d4e5f60789")]
    [InlineData("507f1f77bcf86cd799439011")]
    [InlineData("60d5db2f8e4b0c7d2a3f1e99")]
    public async Task VideoCheckerGetJobByObjectId_ShouldReturnAJob(string stringObjectId)
    {
        var objectId = ObjectId.Parse(stringObjectId);
        var response = new VideoJobStatusChanged(
                ObjectId.GenerateNewId(),
                ObjectId.GenerateNewId(),
                StatusEnum.Pending,
                "message",
                "video.mp4",
                DateTime.Now);

        var setup = _repo.Setup(r => r.GetByObjectId(objectId))
            .ReturnsAsync(response);

        var result = await _repo.Object.GetByObjectId(objectId);

        Assert.NotNull(result);
    }

    [Theory]
    [InlineData(1024)]
    [InlineData(10 * 1024)]
    [InlineData(10 * 1024 * 1024)]
    public async Task VideoCheckerUploadVideo_ShouldReturnObjectId_WhenValidMp4(int tamanhoArquivo)
    {
        var file = FormFileHelper.BuildFormFile(new byte[tamanhoArquivo], "video.mp4");

        var setup = _service.Setup(s => s.UploadVideo(file))
           .ReturnsAsync(ObjectId.GenerateNewId());

        var id = await _service.Object.UploadVideo(file);

        Assert.NotEqual(ObjectId.Empty, id);
    }

    [Fact]
    public async Task VideoCheckerUploadVideo_ShouldThrow_WhenEmptyFile()
    {
        var file = FormFileHelper.BuildEmptyFile("empty.mp4");

        var id = await _service.Object.UploadVideo(file);

        Assert.Equal(ObjectId.Empty, id);
    }

    [Theory]
    [MemberData(nameof(MemberDataGetAll.GetVideoJobStatusChangedData), MemberType = typeof(MemberDataGetAll))]
    public async Task VideoCheckerGetAllJobs_ShouldReturnListOfJobs(List<VideoJobStatusChanged> list)
    {
        var setup = _repo.Setup(r => r.GetAllJobs())
            .ReturnsAsync(list);

        var result = await _repo.Object.GetAllJobs();

        Assert.NotNull(result);
        Assert.Equal(list.Count, result.Count);
    }

    [Theory]
    [MemberData(nameof(MemberDataQrCodeFound.GetVideoJobStatusChangedData), MemberType = typeof(MemberDataQrCodeFound))]
    public async Task VideoCheckerGetQrCodesFound_ShouldReturnAllQRCodeFound(string stringObjectId, List<QrCodeFound> lista)
    {
        var objectId = ObjectId.Parse(stringObjectId);

        var setup = _repo.Setup(r => r.GetQrCodeFounds(objectId))
            .ReturnsAsync(lista);

        var result = await _repo.Object.GetQrCodeFounds(objectId);

        Assert.NotNull(result);
        Assert.Equal(lista.Count, result.Count);
    }
}
