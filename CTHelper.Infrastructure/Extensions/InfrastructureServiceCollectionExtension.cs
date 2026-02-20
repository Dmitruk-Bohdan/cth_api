using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CTHelper.Application.Extensions;
using CTHelper.Persistence.Extensions;
using CTHelper.Infrastructure.Startup;
using CTHelper.Application.ServiceInterfaces;
using CTHelper.Infrastructure.ServiceImplementations;

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
                .AddPersistance(configuration)
                .AddInfrastructureServices();


            return services;
        }

        private static IServiceCollection AddInfrastructureServices(
                    this IServiceCollection services)
        {
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            return services;
        }
    }
}
