using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using CTHelper.Application.Behaviors;

namespace CTHelper.Application.Extensions;

public static class ApplicationServiceCollectionExtension
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        var assembly = typeof(ApplicationServiceCollectionExtension).Assembly;

        services
            .AddValidation(assembly)
            .AddMediatRServices(assembly)
            .AddMapping(assembly);

        return services;
    }

    private static IServiceCollection AddValidation(
        this IServiceCollection services,
        Assembly assembly)
    {
        services.AddValidatorsFromAssembly(assembly);
        return services;
    }

    private static IServiceCollection AddMediatRServices(
        this IServiceCollection services,
        Assembly assembly)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        return services;
    }

    private static IServiceCollection AddMapping(
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
