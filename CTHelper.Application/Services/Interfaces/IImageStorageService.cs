namespace CTHelper.Application.Services.Interfaces
{
    public interface IFileStorageService
    {
        Task DeleteAsync(string key, string bucket);
        Task<Stream> DownloadAsync(string key, string bucket);
        Task<bool> Exists(string key, string bucket);
        string GetDownloadUrl(string key, string bucket);
        string GetUploadUrl(string key, int expiresSeconds, string bucket);
        Task UploadAsync(Stream stream, string key, string contentType, string bucketName);
    }
}
