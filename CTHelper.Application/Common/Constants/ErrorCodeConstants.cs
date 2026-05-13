namespace CTHelper.Application.Common.Constants
{
    public static class ErrorCodeConstants
    {
        //Common codes
        public const string OwnershipRequired = "00000000";

        //User error codes
        public const string UserNotFound = "00010000";
        public const string WrongPassword = "00010001";

        //Email verification error codes
        public const string EmailIsAlreadyVerified = "00020000";
        public const string EmailVerificationTokenIsExpired = "00020001";
        public const string WrongEmailVerificationToken = "00020002";

        //Password reset error codes
        public const string WrongPasswordResetToken = "00030000";
        public const string PasswordResetTokenIsExpired = "00030001";
        public const string NoActivePasswordResetTokenFound = "00030002";

        //Authentication error codes
        public const string InvalidRefreshToken = "00040000";
        public const string RefreshTokenExpired = "00040001";
        public const string DeviceAlreadyHasActiveSession = "00040002";
        public const string InvalidSessionJti = "00040003";
        public const string EmailIsNotVerified = "00040004";

        //Teacher-Student binding codes
        public const string BindingCodeNotFound = "00050000";
        public const string BindingCodeIsRevoked = "00050001";
        public const string StudentIsBlocked = "00050002";
        public const string RelationAlreadyExist = "00050003";
        public const string BindingRequestNotFound = "00050004";
        public const string ForeignBindingConfirmationRequested = "00050005";
        public const string BindingNotFound = "00050006";
        public const string BindingRequestAlreadyAccepted = "00050007";


        //Student Group codes
        public const string GroupNotFound = "00060000";
        public const string StudentNotBelongToGroup = "00060001";
        public const string StudentAlreadyInGroup = "00060002";

        //Favourite codes
        public const string ProblemNotInFavourites = "00070002";
        public const string TestNotInFavourites = "00070003";

        //Notification codes
        public const string NotificationNotFound = "00080000";
        public const string NotificationIdsListIsEmpty = "00080001";

        //Problem codes
        public const string ProblemNotFound = "00090000";

        //Test codes
        public const string TestNotFound = "00100000";

        //Attempt codes
        public const string AttemptNotFound = "00110000";
        public const string AttemptNotActive = "00110001";
        public const string AttemptAlreadyActive = "00110002";
        public const string AttemptIsExaminative = "00110003";

    }
}
