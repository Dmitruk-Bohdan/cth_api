using СTHelper.Application.Extensions;
using СTHelper.Infrastructure;
using СTHelper.Presentation.Extensions;

namespace CTHelper.Presentation;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;
        services.AddInfrastructure(builder.Configuration);
        services.AddApplication(builder.Configuration);

        var app = builder.Build();

        app.ConfigureMiddleware();

        app.Run();
    }
}