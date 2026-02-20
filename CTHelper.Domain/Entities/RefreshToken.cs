namespace CTHelper.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public long SessionId { get; set; }
        public string TokenHash { get; set; } = string.Empty;
        public string DeviceId { get; set; } = string.Empty;
        public DateTimeOffset ExpiresAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? RevokedAt { get; set; }
    }
}
