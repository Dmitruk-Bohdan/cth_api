using Microsoft.AspNetCore.RateLimiting;
using static CTHelper.Presentation.Common.Constants.PoliciesValueConstants;
using static CTHelper.Presentation.Common.Constants.PoliciesNamesConstants;
using CTHelper.Domain.Common.Enums;

namespace CTHelper.Presentation.Policies
{
    public static class AuthPolicies
    {
        public static IServiceCollection AddAuthPolicies(
            this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter(OtpDeliveryPolicy, limiterOptions =>
                {
                    limiterOptions.PermitLimit = OtpDeliveryPermitLimit;
                    limiterOptions.Window = TimeSpan.FromSeconds(OtpDeliveryWindowTimespanSeconds);
                });

                options.AddFixedWindowLimiter(ResendEmailPolicy, limiterOptions =>
                {
                    limiterOptions.PermitLimit = ResendEmailPermitLimit;
                    limiterOptions.Window = TimeSpan.FromHours(ResendEmailWindowTimespanHours);
                });
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(TeacherOnlyPolicy, policy =>
                {
                    policy.RequireRole(UserRoleEnum.Teacher.ToString(), UserRoleEnum.Admin.ToString());
                });
                options.AddPolicy(StudentOnlyPolicy, policy =>
                {
                    policy.RequireRole(UserRoleEnum.Student.ToString(), UserRoleEnum.Admin.ToString());
                });
            });

            return services;
        }
    }
}
