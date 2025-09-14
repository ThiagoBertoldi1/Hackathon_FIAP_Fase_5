using ZXing;
using ZXing.Common;
using ZXingImageSharpReader = ZXing.ImageSharp.BarcodeReader<SixLabors.ImageSharp.PixelFormats.Rgba32>;

namespace VideoChecker.Processor.Utils;
public static class QrFromFrames
{
    public sealed record QrHit(string File, string Text);

    public static async Task<IReadOnlyList<QrHit>> DetectQrInDirAsync(string framesDir)
    {
        var reader = new ZXingImageSharpReader
        {
            Options = new DecodingOptions
            {
                PossibleFormats = [BarcodeFormat.QR_CODE],
                TryHarder = true,
                PureBarcode = false
            },
            AutoRotate = true,
            TryInverted = true
        };

        var hits = new List<QrHit>();

        var files = Directory.EnumerateFiles(framesDir, "*.png")
                             .OrderBy(p => p, StringComparer.OrdinalIgnoreCase);

        foreach (var file in files)
        {
            using var img = await Image.LoadAsync<Rgba32>(file);
            var result = reader.Decode(img);

            if (result is not null)
                hits.Add(new QrHit(file, result.Text));
        }

        return hits;
    }
}