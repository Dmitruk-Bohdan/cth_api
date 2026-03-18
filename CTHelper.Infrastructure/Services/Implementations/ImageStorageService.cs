using Amazon.S3;
using Amazon.S3.Model;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Presentation.Settings;
using Microsoft.Extensions.Options;
using System.Net;

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


        public async Task UploadAsync(
            Stream stream,
            string key,
            string contentType,
            string bucketName)
        {
            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = key,
                InputStream = stream,
                ContentType = contentType
            };

            await _s3.PutObjectAsync(request);
        }

        public async Task<Stream> DownloadAsync(string key, string bucket)
        {
            var response = await _s3.GetObjectAsync(bucket, key);
            return response.ResponseStream;
        }

        public async Task DeleteAsync(string key, string bucket)
        {
            var response = await _s3.DeleteObjectAsync(bucket, key);
        }

        public async Task<bool> Exists(string key, string bucket)
        {
            try
            {
                await _s3.GetObjectMetadataAsync(bucket, key);
                return true;
            }
            catch (AmazonS3Exception e)
            {
                return e.StatusCode != HttpStatusCode.NotFound;
            }
        }

        public string GetDownloadUrl(string key, string bucket)
            => CreateFileUrl(key, HttpVerb.GET, _settings.GetFileLinkExpirationSeconds, bucket);

        public string GetUploadUrl(string key, int expiresSeconds, string bucket)
            => CreateFileUrl(key, HttpVerb.PUT, _settings.PutFileLinkExpirationSeconds, bucket);

        private string CreateFileUrl(string key, HttpVerb verb, int seconds, string bucket)
        {
            return _s3.GetPreSignedURL(new GetPreSignedUrlRequest
            {
                BucketName = bucket,
                Key = key,
                Verb = verb,
                Expires = DateTime.UtcNow.AddSeconds(seconds)
            });
        }
    }
}
