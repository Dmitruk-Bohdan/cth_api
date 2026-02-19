using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CTHelper.Application.Extensions;
using CTHelper.Persistence.Extensions;
using CTHelper.Infrastructure.Startup;

namespace CTHelper.Infrastructure
{
    public static class InfrastructureServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddApplication()
                .AddPersistance(configuration);

            return services;
        }
        
    }
}
