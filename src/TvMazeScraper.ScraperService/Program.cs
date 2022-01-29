using TvMazeScraper.Application.IoC;
using TvMazeScraper.Infrastructure.IoC;
using TvMazeScraper.ScraperService;
using TvMazeScraper.ScraperService.Bootstrap;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddInfrastructure(true);
        services.AddApplication(true);
        services.AddHostedService<Worker>();
    })
    .Build();

await Bootstrapper.RunAsync(host);

await host.RunAsync();


