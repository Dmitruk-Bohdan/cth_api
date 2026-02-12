using СTHelper.Domain.Common.Enums;

namespace СTHelper.Domain.Entities
{
    public class UserSession : BaseEntity
    {
        public long UserId { get; set; }

        public Guid Jti { get; set; }

        public ClientType ClientType { get; set; }

        public string IpAddress { get; set; } = default!;

        public string DeviceInfo { get; set; } = default!;

        public DateTimeOffset LastActivityAt { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset LastUpdateAt { get; set; }

        public DateTimeOffset? RevokedAt { get; set; }

        public User User { get; set; } = default!;
    }
}
