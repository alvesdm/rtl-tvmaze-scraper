using Microsoft.Extensions.DependencyInjection;
using TvMazeScraper.Application.Interfaces;

namespace TvMazeScraper.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddHttpClient<ITVShowApiClient, TVShowApiClient>();
        services.AddTransient<ITVShowScraper, TVMazeShowScraper>();
        services.AddTransient<IShowService, ShowService>();
        return services;
    }
}