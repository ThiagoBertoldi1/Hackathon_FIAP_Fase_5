using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace VideoChecker.Test.Helper;
public static class FormFileHelper
{
    public static IFormFile BuildFormFile(byte[] content, string fileName, string contentType = "video/mp4")
    {
        var ms = new MemoryStream(content);
        return new FormFile(ms, 0, ms.Length, name: "file", fileName: fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };
    }

    public static IFormFile BuildEmptyFile(string fileName = "empty.mp4", string contentType = "video/mp4")
        => BuildFormFile([], fileName, contentType);
}