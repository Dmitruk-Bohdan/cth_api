using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace СTHelper.Persistence.Extensions
{
    public static class PersistanceServiceCollectionExtension
    {
        public static IServiceCollection AddPersistance(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services;
        }
    }
}
