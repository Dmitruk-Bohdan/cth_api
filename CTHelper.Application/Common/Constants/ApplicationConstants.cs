namespace CTHelper.Application.Common.Constants
{
    public static class ApplicationConstants
    {
        public static int EmailVerificationTokenUpperBound = 999999;
        public static int EmailVerificationTokenBottomBound = 0;

        public static double EmailVerificationTokenLifetimeSeconds = 300;
        public static short AttemptsLimitToValidateEmailVerificationByOneToken = 5;

        public static double PasswordResetTokenLifetimeSeconds = 300;
        public static short AttemptsLimitToValidatePasswordResetByOneToken = 3;

    }
}
