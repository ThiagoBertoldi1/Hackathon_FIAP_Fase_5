using FFMpegCore;

namespace VideoChecker.Processor.Utils;

public static class VideoFrames
{
    public static async Task SaveFramesAsPngAsync(Stream mongoStream, string outputDir)
    {
        Directory.CreateDirectory(outputDir);
        var outputPattern = Path.Combine(outputDir, "frame_%06d.png");

        var tmp = Path.Combine(Path.GetTempPath(), $"gridfs_{Guid.NewGuid():N}.mp4");
        try
        {
            await using (var fs = File.Create(tmp))
                await mongoStream.CopyToAsync(fs);

            var args = FFMpegArguments
                .FromFileInput(tmp, verifyExists: true)
                .OutputToFile(outputPattern, overwrite: true, o =>
                {
                    o.WithVideoCodec("png");
                    o.WithCustomArgument("-an");
                });

            await args.ProcessAsynchronously();
        }
        finally
        {
            try { File.Delete(tmp); } catch { }
        }
    }
}