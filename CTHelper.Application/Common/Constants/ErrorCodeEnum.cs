namespace CTHelper.Application.Common.Constants
{
    public static class ErrorCodeConstants
    {
        //User error codes
        public const string UserDoesntExist = "00001000";
        public const string WrongPassword = "00001001";
        public const string UserNotFound = "00001002";

        //Email verification error codes
        public const string EmailIsAlreadyVerified = "00002000";
        public const string EmailVerificationTokenIsExpired = "00002001";
        public const string WrongEmailVerificationToken = "00002002";

        //Password reset error codes
        public const string WrongPasswordResetToken = "00004000";
        public const string PasswordResetTokenIsExpired = "00004001";
        public const string NoActivePasswordResetTokenFound = "00004002";

        //Authentication error codes
        public const string InvalidRefreshToken = "00003000";
        public const string RefreshTokenExpired = "00003001";
        public const string DeviceAlreadyHasActiveSession = "00003002";
    }
}
