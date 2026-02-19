using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CTHelper.Persistence.Context;
using static CTHelper.Persistence.Common.Constants.DbConfigurationKeys;

namespace CTHelper.Persistence.Extensions;

public static class PersistanceServiceCollectionExtension
{
    public static IServiceCollection AddPersistance(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        var connectionString = configuration.GetConnectionString(DefaultConnection);

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(
                connectionString,
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsAssembly(MigrationsAssembly);
                });
        });

        return services;
    }
}
