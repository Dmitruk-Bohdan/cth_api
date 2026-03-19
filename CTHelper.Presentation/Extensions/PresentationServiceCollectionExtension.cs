using CTHelper.Application.Extensions;
using CTHelper.Infrastructure;
using CTHelper.Infrastructure.Settings;
using CTHelper.Infrastructure.Startup;
using CTHelper.Presentation.Routing;
using CTHelper.Presentation.Security;
using CTHelper.Presentation.Settings;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace CTHelper.Presentation.Extensions
{
    public static class PresentationServiceCollectionExtension
    {
        public static IServiceCollection AddPresentation(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddInfrastructure(configuration);

            var assembly = typeof(PresentationServiceCollectionExtension).Assembly;
            services.AddPresentationMapping(assembly);

            services.AddRoutingConfiguration()
                .AddHttpContextAccessor()
                .AddCorsPolicy()
                .AddPresentationSettings()
                .AddPolicies()
                .AddBearerAuthentication(configuration)
                .AddSwaggerConfiguration();

            return services;
        }

        public static IServiceCollection AddBearerAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();

            services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
        .AddJwtBearer(options =>
        {
            options.IncludeErrorDetails = true;

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    Console.WriteLine("TOKEN RECEIVED:");
                    Console.WriteLine(context.Request.Headers["Authorization"]);
                    return Task.CompletedTask;
                },

                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine("AUTH FAILED:");
                    Console.WriteLine(context.Exception.ToString());
                    return Task.CompletedTask;
                },

                OnTokenValidated = context =>
                {
                    Console.WriteLine("TOKEN VALIDATED");
                    return Task.CompletedTask;
                },

                OnChallenge = context =>
                {
                    Console.WriteLine("CHALLENGE:");
                    Console.WriteLine(context.Error);
                    Console.WriteLine(context.ErrorDescription);
                    return Task.CompletedTask;
                }
            };

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings!.Issuer,
                ValidAudience = jwtSettings.Audience,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
            };
        });

            return services;
        }
        private static IServiceCollection AddPresentationMapping(
        this IServiceCollection services,
        Assembly assembly)
        {
            var config = new TypeAdapterConfig();
            config.Scan(assembly);

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }
    }
}
