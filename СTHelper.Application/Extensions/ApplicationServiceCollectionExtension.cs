using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace СTHelper.Application.Extensions
{
    public static class ApplicationServiceCollectionExtension
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services;
        }
    }
}
