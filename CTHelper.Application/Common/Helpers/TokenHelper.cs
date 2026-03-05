using static CTHelper.Application.Common.Constants.ApplicationConstants;

namespace CTHelper.Application.Common.Helpers
{
    public static class TokenHelper
    {
        public static string Get6NumbersToken()
        {
            var random = new Random();
            var tokenAsNumber = random.Next(
                EmailVerificationTokenBottomBound,
                EmailVerificationTokenUpperBound);
            return tokenAsNumber.ToString("D6");
        }
    }
}
