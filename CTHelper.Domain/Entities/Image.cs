namespace CTHelper.Domain.Entities
{
    public class Image : BaseEntity
    {
        public string ObjectKey { get; set; } = null!;
        public string Bucket { get; set; } = null!;

        public long OwnerId { get; set; }

        public string ContentType { get; set; } = null!;

        public long Size { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public User Owner { get; set; } = default!;
    }
}
