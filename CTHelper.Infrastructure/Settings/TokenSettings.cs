namespace CTHelper.Infrastructure.Settings
{
    public class TokenSettings
    {
        public int EmailVerificationTokenUpperBound { get; set; }
        public int EmailVerificationTokenBottomBound { get; set; }
        public double EmailVerificationTokenLifetimeSeconds { get; set; }
        public short AttemptsLimitToValidateEmailVerificationByOneToken { get; set; }
        public double PasswordResetTokenLifetimeSeconds { get; set; }
        public short AttemptsLimitToValidatePasswordResetByOneToken { get; set; }
        public string ShortTokenSecretKey { get; set; } = default!;
        public string RefreshTokenSecretKey { get; set; } = default!;
    }
}
