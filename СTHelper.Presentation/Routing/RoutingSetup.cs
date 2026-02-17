using Microsoft.Extensions.DependencyInjection;

namespace СTHelper.Presentation.Routing;

public static class RoutingSetup
{
    public static IServiceCollection AddRoutingConfiguration(this IServiceCollection services)
    {
        services. AddControllers();
        services.AddHttpContextAccessor();
        services.AddRouting();

        return services;
    }
}