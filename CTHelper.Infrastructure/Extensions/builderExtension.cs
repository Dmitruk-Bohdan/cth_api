using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using static CTHelper.Infrastructure.Common.Constants.ConfigurationSourceKeys;

namespace CTHelper.Infrastructure.Extensions
{
    public static class WebApplicationBuilderExtension
    {
        public static void SetUpConfigurationSources(this WebApplicationBuilder builder)
        {
            var envAppsettingFileName = $"appsettings.{builder.Environment.EnvironmentName}.json";

            builder.Configuration
                .AddJsonFile(AppSettings, optional: false, reloadOnChange: true)
                .AddJsonFile(envAppsettingFileName, optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        }
    }
}
