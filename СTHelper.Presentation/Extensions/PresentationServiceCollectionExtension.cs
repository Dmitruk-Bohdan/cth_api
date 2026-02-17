using СTHelper.Presentation.Routing;
using СTHelper.Presentation.Security;
using СTHelper.Presentation.Settings;

namespace СTHelper.Presentation.Extensions
{
    public static class PresentationServiceCollectionExtension
    {
        public static IServiceCollection AddPresentation(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddRoutingConfiguration()
                .AddHttpContextAccessor()
                .AddCorsPolicy()
                .AddPresentationSettings()
                ;

            return services;
        }
    }
}
