namespace CTHelper.Domain.Entities
{
    public class EmailVerificationToken : BaseEntity
    {
        public long UserId { get; set; }
        public string TokenHash { get; set; } = string.Empty;
        public DateTimeOffset ExpiresAt { get; set; }
        public DateTimeOffset? VerifiedAt { get; set; }

        public User User { get; set; } = default!;
    }
}
