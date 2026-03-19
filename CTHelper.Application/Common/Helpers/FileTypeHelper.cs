namespace CTHelper.Application.Common.Helpers
{
    public static class FileTypeHelper
    {
        private static readonly Dictionary<string, string> Map = new()
        {
            ["image/jpeg"] = ".jpg",
            ["image/png"] = ".png",
            ["image/webp"] = ".webp",
            ["image/gif"] = ".gif"
        };

        public static string GetExtension(string contentType)
        {
            if (Map.TryGetValue(contentType.ToLowerInvariant(), out var ext))
                return ext;

            throw new NotSupportedException($"Unsupported content type: {contentType}");
        }
    }
}