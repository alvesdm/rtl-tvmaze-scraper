using TvMazeScraper.Application.Interfaces;

namespace TvMazeScraper.ScraperService.Bootstrap;
internal class Bootstrapper
{
    internal static async Task RunAsync(IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                var databaseBootstrapper = services.GetRequiredService<IDatabaseBootstrapper>();
                await databaseBootstrapper.SetupAsync();

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating or seeding the database.");

                throw;
            }
        }
    }
}