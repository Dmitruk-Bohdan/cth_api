using CTHelper.Infrastructure;
using CTHelper.Infrastructure.Startup;
using CTHelper.Presentation.Routing;
using CTHelper.Presentation.Security;
using CTHelper.Presentation.Settings;

namespace CTHelper.Presentation.Extensions
{
    public static class PresentationServiceCollectionExtension
    {
        public static IServiceCollection AddPresentation(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddInfrastructure(configuration);

            services.AddRoutingConfiguration()
                .AddHttpContextAccessor()
                .AddCorsPolicy()
                .AddPresentationSettings()
                .AddSwaggerConfiguration();

            return services;
        }
    }
}
