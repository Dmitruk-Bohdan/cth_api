using CTHelper.Presentation.Policies;

namespace CTHelper.Presentation.Extensions
{
    public static class PoliciesExtension
    {
        public static IServiceCollection AddPolicies(
            this IServiceCollection services)
        {
            services.AddAuthPolicies();
            return services;
        }
    }
}
