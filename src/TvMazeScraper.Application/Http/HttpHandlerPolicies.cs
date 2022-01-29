using Polly;
using Polly.Extensions.Http;

namespace TvMazeScraper.Application.Http;

public class HttpHandlerPolicies
{
    public static IAsyncPolicy<HttpResponseMessage> Get409RetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                        retryAttempt)));
    }
}