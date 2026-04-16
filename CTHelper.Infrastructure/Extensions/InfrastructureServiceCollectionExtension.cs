using Amazon.S3;
using CTHelper.Application.Extensions;
using CTHelper.Application.Services.Implementations;
using CTHelper.Application.Services.Interfaces;
using CTHelper.Infrastructure.Services.Implementations;
using CTHelper.Infrastructure.Settings;
using CTHelper.Persistence.Extensions;
using CTHelper.Presentation.Settings;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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
                .AddInfrastructureServices()
                .AddS3Storage(configuration);



            return services;
        }

        private static IServiceCollection AddInfrastructureServices(
                    this IServiceCollection services)
        {
            services.AddScoped<IEmailService, MailHogService>();
            services.AddScoped<IIdentityTokenService, IdentityTokenService>();
            services.AddTransient<IPasswordHashingService, PasswordHasherAdapter>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddTransient<IIdentityTokenService, IdentityTokenService>();
            services.AddTransient<IShortTokenService, ShortTokenService>();
            services.AddScoped<IFileStorageService, MinioFileStorageService>();
            services.AddScoped<IUserManagmentService, UserManagmentService>();
            services.AddScoped<ITeacherStudentService, TeacherStudentService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IAssignmentService, AssignmentService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IFavouriteService, FavouriteService>();
            services.AddScoped<IProblemService, ProblemService>();
            services.AddScoped<IStatisticsService, StatisticsService>();
            services.AddScoped<ITestService, TestService>();
            services.AddScoped<ITestAttemptService, TestAttemptService>();

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
            services.Configure<S3Settings>(configuration.GetSection(nameof(S3Settings)));
            return services;
        }

        private static IServiceCollection AddS3Storage(
                    this IServiceCollection services,
                    IConfiguration configuration)
        {
            var s3Settings = configuration.GetSection(nameof(S3Settings)).Get<S3Settings>();

            services.AddSingleton<IAmazonS3>(_ =>
            {
                var config = new AmazonS3Config
                {
                    ServiceURL = s3Settings!.Endpoint,
                    ForcePathStyle = s3Settings.ForcePathStyle,
                    UseHttp = !s3Settings.UseSsl
                };

                return new AmazonS3Client(s3Settings.AccessKey, s3Settings.SecretKey, config);
            });

            return services;
        }
    }
}
