using Amazon.S3;
using Amazon.S3.Model;
using CTHelper.Application.Common.Helpers;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Application.Specification.ImageSpecifications;
using CTHelper.Domain.Abstractions;
using CTHelper.Presentation.Settings;
using Microsoft.Extensions.Options;
using System.Net;

namespace CTHelper.Infrastructure.Services.Implementations
{
    public class MinioFileStorageService : IFileStorageService
    {
        private readonly IAmazonS3 _s3;
        private readonly S3Settings _settings;
        private readonly IUnitOfWork _unitOfWork;
        public MinioFileStorageService(IAmazonS3 s3,
            IOptions<S3Settings> settings,
            IUnitOfWork unitOfWork)
        {
            _s3 = s3;
            _settings = settings.Value;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> UploadAsync(
            Stream stream,
            string keyPrefix,
            string bucketName,
            string contentType)
        {
            var extension = FileTypeHelper.GetExtension(contentType);
            var key = $"{keyPrefix}/{DateTime.UtcNow:yyyy/MM/dd}/{Guid.NewGuid()}{extension}";

            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = key,
                InputStream = stream,
                ContentType = contentType
            };

            await _s3.PutObjectAsync(request);

            return key;
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

        public async Task<string> GetDownloadUrl(long imageId)
            => await CreateFileUrl(imageId, HttpVerb.GET, _settings.GetFileLinkExpirationSeconds);

        public async Task<string> GetUploadUrl(long imageId)
            => await CreateFileUrl(imageId, HttpVerb.PUT, _settings.PutFileLinkExpirationSeconds);

        private async Task<string> CreateFileUrl(long imageId, HttpVerb verb, int seconds)
        {
            var imageCreds = await _unitOfWork.Images.GetAsync(new GetImageCredsByImageIdSpecification(imageId));
            if(imageCreds == null)
            {
                throw new ArgumentException($"Image {imageId} not found!");
            }

            return _s3.GetPreSignedURL(new GetPreSignedUrlRequest
            {
                BucketName = imageCreds.Bucket,
                Key = imageCreds.ObjectKey,
                Verb = verb,
                Expires = DateTime.UtcNow.AddSeconds(seconds)
            });
        }
    }
}
