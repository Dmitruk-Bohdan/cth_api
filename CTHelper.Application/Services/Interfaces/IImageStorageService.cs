namespace CTHelper.Application.Services.Interfaces
{
    public interface IFileStorageService
    {
        Task DeleteAsync(string key, string bucket);
        Task<Stream> DownloadAsync(string key, string bucket);
        Task<bool> Exists(string key, string bucket);
        Task<string> GetDownloadUrl(long imageId);
        Task<string> GetUploadUrl(long imageId);
        Task<string> UploadAsync(Stream stream, string keyprefix, string bucketName, string contentType);
    }
}
