namespace CTHelper.Domain.Entities
{
    public class EmailVerificationToken : BaseEntity
    {
        public long UserId { get; set; }
        public string TokenHash { get; set; } = string.Empty;
        public short AttemptsLeft { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
        public DateTimeOffset? VerifiedAt { get; set; } = default;

        public User User { get; set; } = default!;
    }
}
