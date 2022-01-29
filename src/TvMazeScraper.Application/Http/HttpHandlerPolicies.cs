using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace TvMazeScraper.Application.Http;

public class HttpHandlerPolicies
{
    public static IAsyncPolicy<HttpResponseMessage> Get409RetryPolicy(IServiceProvider services)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            .WaitAndRetryAsync(
            6, 
            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            (message, timespan) => {
                var logger = services.GetService<ILogger<HttpHandlerPolicies>>();
                logger.LogWarning("Ooops! Too fast. Backing off a bit for {@Time} seconds.", timespan.TotalSeconds);
            });
    }
}