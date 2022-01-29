using TvMazeScraper.Application;
using TvMazeScraper.Infrastructure;
using TvMazeScraper.ScraperService;
using TvMazeScraper.ScraperService.Bootstrap;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddInfrastructure();
        services.AddApplication();
        services.AddHostedService<Worker>();
    })
    .Build();

await Bootstrapper.RunAsync(host);

await host.RunAsync();


