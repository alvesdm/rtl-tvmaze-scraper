using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TvMazeScraper.Application.Business.Repository;
using TvMazeScraper.Application.Business.Services;
using TvMazeScraper.Application.Business.Shows;
using TvMazeScraper.Application.Http;
using TvMazeScraper.Application.Interfaces;

namespace TvMazeScraper.Application.IoC;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, bool isConsoleApp = false)
    {
        if (!isConsoleApp)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient<IShowService, ShowService>();
        }
        else
        {
            services.AddHttpClient<ITVShowApiClient, TVShowApiClient>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler((services, request) => HttpHandlerPolicies.Get409RetryPolicy(services));
            services.AddTransient<ITVShowScraper, TVMazeShowScraper>();
            services.AddTransient<IScraperRepository, ScraperRepository>();
        }

        return services;
    }
}
