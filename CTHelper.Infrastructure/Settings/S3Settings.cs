namespace CTHelper.Presentation.Settings
{
    public class S3Settings
    {
        public string Endpoint { get; set; } = default!;
        public string AccessKey { get; set; } = default!;
        public string SecretKey { get; set; } = default!;
        public string AvatarBucket { get; set; } = default!;
        public string ProblemImagesBucket { get; set; } = default!;
        public bool UseSsl { get; set; }
        public bool ForcePathStyle { get; set; }
        public int GetFileLinkExpirationSeconds { get; set; }
        public int PutFileLinkExpirationSeconds { get; set; }

    }
}
