namespace CTHelper.Application.Common.Enums
{
    public static class ErrorCodeConstants
    {
        //Common codes
        public const string OwnershipRequired = "00000000";

        //User error codes
        public const string UserNotFound = "00001000";
        public const string WrongPassword = "00001001";

        //Email verification error codes
        public const string EmailIsAlreadyVerified = "00002000";
        public const string EmailVerificationTokenIsExpired = "00002001";
        public const string WrongEmailVerificationToken = "00002002";

        //Password reset error codes
        public const string WrongPasswordResetToken = "00003000";
        public const string PasswordResetTokenIsExpired = "00003001";
        public const string NoActivePasswordResetTokenFound = "00003002";

        //Authentication error codes
        public const string InvalidRefreshToken = "00004000";
        public const string RefreshTokenExpired = "00004001";
        public const string DeviceAlreadyHasActiveSession = "00004002";
        public const string InvalidSessionJti = "00004003";
        public const string EmailIsNotVerified = "00004004";

        //Teacher-Student binding codes
        public const string BindingCodeNotFound = "00005000";
        public const string BindingCodeIsRevoked = "00005001";
        public const string StudentIsBlocked = "00005002";
        public const string RelationAlreadyExist = "00005003";
        public const string BindingRequestNotFound = "00005004";
        public const string ForeignBindingConfirmationRequested = "00005005";
        public const string BindingNotFound = "00005006";
    }
}
