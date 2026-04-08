using Booking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Booking.Infrastructure.Extensions;

public static class MigrationExtensions
{
    /// <summary>
    /// Applies any pending EF Core migrations to the database.
    /// Creates the database if it does not exist.
    /// </summary>
    public static async Task ApplyMigrationsAsync(this IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

        try
        {
            var pending = await dbContext.Database.GetPendingMigrationsAsync();
            var migrations = pending.ToList();

            if (migrations.Count != 0)
            {
                logger.LogInformation("Applying {Count} pending migration(s)...", migrations.Count);
                await dbContext.Database.MigrateAsync();
                logger.LogInformation("Migrations applied successfully.");
            }
            else
            {
                logger.LogInformation("Database is up to date. No pending migrations.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while applying migrations.");
            throw;
        }
    }
}
