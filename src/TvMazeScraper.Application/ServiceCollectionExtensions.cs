using Microsoft.Extensions.DependencyInjection;
using TvMazeScraper.Application.Http;
using TvMazeScraper.Application.Interfaces;

namespace TvMazeScraper.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddHttpClient<ITVShowApiClient, TVShowApiClient>()
            .SetHandlerLifetime(TimeSpan.FromMinutes(5))
            .AddPolicyHandler(HttpHandlerPolicies.Get409RetryPolicy());
        services.AddTransient<ITVShowScraper, TVMazeShowScraper>();
        services.AddTransient<IShowService, ShowService>();
        return services;
    }
}
