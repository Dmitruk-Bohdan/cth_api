using CTHelper.Infrastructure.Extensions;
using CTHelper.Presentation.Extensions;
using Microsoft.IdentityModel.Logging;

namespace CTHelper.Presentation;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.SetUpConfigurationSources();
        var configuration = builder.Configuration;

        var services = builder.Services;

        services.AddPresentation(configuration);

        IdentityModelEventSource.ShowPII = true;
        var app = builder.Build();

        app.ConfigureMiddleware();


        app.Run();
    }
}