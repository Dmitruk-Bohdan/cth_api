using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using СTHelper.Application.Extensions;
using СTHelper.Persistence.Extensions;

namespace СTHelper.Infrastructure
{
    public static class InfrastructureServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddApplication(configuration)
                .AddPersistance(configuration);

            return services;
        }
    }
    
}
