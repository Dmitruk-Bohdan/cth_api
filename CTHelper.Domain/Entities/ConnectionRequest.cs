namespace CTHelper.Domain.Entities
{
    public class ConnectionRequest : BaseEntity
    {
        public long CodeId { get; set; }
        public long StudentId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdateAt { get; set; }

        public InvitationCode Code { get; set; } = default!;
        public User Student { get; set; } = default!;
    }
}
