namespace CTHelper.Domain.Entities
{
    public class PasswordResetToken : BaseEntity
    {
        public long UserId { get; set; }
        public string TokenHash { get; set; } = string.Empty;
        public short AttemptsLeft { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
        public DateTimeOffset? UsedAt { get; set; }
    }
}
