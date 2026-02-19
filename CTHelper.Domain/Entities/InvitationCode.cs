namespace CTHelper.Domain.Entities
{
    public class InvitationCode : BaseEntity
    {
        public long TeacherId { get; set; }

        public string Code { get; set; } = default!;

        public short UsesLeft { get; set; }

        public bool IsRevoked { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset LastUpdateAt { get; set; }

        public User Teacher { get; set; } = default!;
    }
}
