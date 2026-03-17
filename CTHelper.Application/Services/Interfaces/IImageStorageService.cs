namespace CTHelper.Application.Services.Interfaces
{
    public interface IFileStorageService
    {
        Task<List<string>> GetFileNamesAsync(string? prefix = null);
    }
}
