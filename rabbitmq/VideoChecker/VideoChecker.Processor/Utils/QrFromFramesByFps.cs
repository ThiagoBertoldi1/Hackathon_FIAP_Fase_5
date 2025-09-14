using ZXing;
using ZXing.Common;
using ZXingImageSharpReader = ZXing.ImageSharp.BarcodeReader<SixLabors.ImageSharp.PixelFormats.Rgba32>;

namespace VideoChecker.Processor.Utils;

public static class QrFromFramesByFps
{
    public sealed record QrHit(
        int FrameIndex,
        double TimestampSeconds,
        string Text,
        string File
    );

    public static async Task<IReadOnlyList<QrHit>> DetectAllAsync(string framesDir, CancellationToken ct)
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

        var files = Directory.EnumerateFiles(framesDir, "*.png")
                             .OrderBy(p => p, StringComparer.OrdinalIgnoreCase)
                             .ToArray();

        var hits = new List<QrHit>(files.Length);
        for (int i = 0; i < files.Length; i++)
        {
            ct.ThrowIfCancellationRequested();
            using var img = await Image.LoadAsync<Rgba32>(files[i], ct);

            var results = reader.DecodeMultiple(img);
            if (results is null || results.Length == 0) continue;

            foreach (var r in results.GroupBy(r => r.Text).Select(g => g.First()))
            {
                if (string.IsNullOrWhiteSpace(r.Text)) continue;
                var t = Math.Round(i / 30f, 2, MidpointRounding.AwayFromZero);
                hits.Add(new QrHit(i, t, r.Text!, files[i]));
            }
        }
        return hits;
    }

    public static IReadOnlyList<(string Text, double FirstTimestampSeconds)> FirstSeenPerText(IEnumerable<QrHit> hits)
        => hits.GroupBy(h => h.Text)
               .Select(g => (g.Key, g.Min(h => h.TimestampSeconds)))
               .OrderBy(t => t.Item2)
               .ToList();
}