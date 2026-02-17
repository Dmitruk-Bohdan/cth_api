using СTHelper.Application.Extensions;
using СTHelper.Infrastructure;
using СTHelper.Presentation.Extensions;

namespace CTHelper.Presentation;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;
        var services = builder.Services;

        services.AddPresentation(configuration);

        var app = builder.Build();

        app.ConfigureMiddleware();

        app.Run();
    }
}