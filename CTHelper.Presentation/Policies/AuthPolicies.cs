using Microsoft.AspNetCore.RateLimiting;
using static CTHelper.Presentation.Common.Constants.PoliciesValueConstants;
using static CTHelper.Presentation.Common.Constants.PoliciesNamesConstants;

namespace CTHelper.Presentation.Policies
{
    public static class AuthPolicies
    {
        public static IServiceCollection AddAuthPolicies(
            this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter(EmailVerificationPolicy, limiterOptions =>
                {
                    limiterOptions.PermitLimit = EmailVerificationPermitLimit;
                    limiterOptions.Window = TimeSpan.FromSeconds(EmailVerificationWindowTimespanSeconds);
                });

                options.AddFixedWindowLimiter("resendEmail", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 3;
                    limiterOptions.Window = TimeSpan.FromHours(1);
                });
            });

            return services;
        }
    }
}
