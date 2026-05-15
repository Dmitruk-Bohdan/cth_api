using Microsoft.EntityFrameworkCore;
using CTHelper.Persistence.Context;

namespace CTHelper.Presentation.Extensions
{
    public static class DatabaseMigrationExtension
    {
        public static void ApplyMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var dbContext = services.GetRequiredService<AppDbContext>();
                var logger = services.GetRequiredService<ILogger<Program>>();

                logger.LogInformation("Checking for pending migrations...");

                var pendingMigrations = dbContext.Database.GetPendingMigrations();
                var pendingMigrationsList = pendingMigrations.ToList();

                if (pendingMigrationsList.Any())
                {
                    logger.LogInformation("Found {Count} pending migration(s). Applying...", pendingMigrationsList.Count);
                    dbContext.Database.Migrate();
                    logger.LogInformation("All migrations applied successfully");
                }
                else
                {
                    logger.LogInformation("No pending migrations. Database is up to date.");
                }
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while applying database migrations");
                throw;
            }
        }
    }
}