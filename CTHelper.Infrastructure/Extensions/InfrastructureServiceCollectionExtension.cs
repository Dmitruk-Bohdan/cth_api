using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CTHelper.Application.Extensions;
using CTHelper.Persistence.Extensions;
using CTHelper.Infrastructure.Services.Implementations;
using CTHelper.Infrastructure.Settings;
using CTHelper.Application.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using CTHelper.Domain.Entities;
using CTHelper.Application.Services.Implementations;

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
            services.AddTransient<IEmailService, MailHogService>();
            services.AddTransient<IIdentityTokenService, IdentityTokenService>();
            services.AddTransient<IPasswordHashingService, PasswordHasherAdapter>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IIdentityTokenService, IdentityTokenService>();
            services.AddTransient<IShortTokenService, ShortTokenService>();

            return services;
        }

        private static IServiceCollection AddInfrastructureSettings(
                    this IServiceCollection services,
                    IConfiguration configuration)
        {
            //ervices.Configure<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));
            services.Configure<EmailSettings>(configuration.GetSection("MailhogSettings"));
            services.Configure<MobileAppSettings>(configuration.GetSection(nameof(MobileAppSettings)));
            services.Configure<TokenSettings>(configuration.GetSection(nameof(TokenSettings)));
            services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
            return services;
        }
    }
}
