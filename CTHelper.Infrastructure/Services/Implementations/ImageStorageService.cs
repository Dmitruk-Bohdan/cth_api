using Amazon.S3;
using Amazon.S3.Model;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Presentation.Settings;
using Microsoft.Extensions.Options;

namespace CTHelper.Infrastructure.Services.Implementations
{
    public class MinioFileStorageService : IFileStorageService
    {
        private readonly IAmazonS3 _s3;
        private readonly S3Settings _settings;
        public MinioFileStorageService(IAmazonS3 s3,
            IOptions<S3Settings> settings)
        {
            _s3 = s3;
            _settings = settings.Value;
        }

        public async Task<List<string>> GetFileNamesAsync(string? prefix = null)
        {
            var fileNames = new List<string>();
            string continuationToken = null!;

            do
            {
                var request = new ListObjectsV2Request
                {
                    BucketName = _settings.AvatarBucket,
                    ContinuationToken = continuationToken,
                    MaxKeys = 10          
                };

                var response = await _s3.ListObjectsV2Async(request);

                fileNames.AddRange(response.S3Objects.Select(o => o.Key));

                continuationToken = (response.IsTruncated ?? false) ? response.NextContinuationToken : null;
            } while (continuationToken != null);

            return fileNames;
        }
    }
}
