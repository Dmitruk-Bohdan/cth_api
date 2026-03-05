using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CTHelper.Application.Extensions;
using CTHelper.Persistence.Extensions;
using CTHelper.Infrastructure.Services.Implementations;
using CTHelper.Infrastructure.Settings;
using CTHelper.Application.Services.Interfaces;

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
                .AddInfrastructureSettings(configuration)
                .AddInfrastructureServices();



            return services;
        }

        private static IServiceCollection AddInfrastructureServices(
                    this IServiceCollection services)
        {
            services.AddTransient<IHashService, HashService>();
            return services;
        }

        private static IServiceCollection AddInfrastructureSettings(
                    this IServiceCollection services,
                    IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));
            services.Configure<MobileAppSettings>(configuration.GetSection(nameof(MobileAppSettings)));
            return services;
        }
    }
}
