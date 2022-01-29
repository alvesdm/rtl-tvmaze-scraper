using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Transactions;
using TvMazeScraper.Application.Extensions;
using TvMazeScraper.Application.Interfaces;
using TvMazeScraper.Domain.Entities;

namespace TvMazeScraper.Application;

public class TVMazeShowScraper : ITVShowScraper
{
    private readonly ILogger<TVMazeShowScraper> _logger;
    private readonly IConfiguration _configuration;
    private readonly ITVShowApiClient _tvShowApiClient;
    private readonly IShowService _showService;

    public TVMazeShowScraper(
        ILogger<TVMazeShowScraper> logger,
        IConfiguration configuration,
        ITVShowApiClient tvShowApiClient,
        IShowService showService)
    {
        _logger = logger;
        _configuration = configuration;
        _tvShowApiClient = tvShowApiClient;
        _showService = showService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var apiPage = await GetApiPageAsync();

        while (!cancellationToken.IsCancellationRequested)
        {
            var result = await _tvShowApiClient.GetShowsPageAsync(apiPage);
            if (!result.IsSuccess)
            {
                break;
            }

            await ProcessShowsAsync(result.Data.ToList());
            apiPage++;
        }
    }

    private async Task ProcessShowsAsync(List<Show> shows)
    {
        await shows.AsyncParallelForEach(async show =>
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                if (await _showService.TryAddShowAsync(show))
                {
                    var castingResult = await _tvShowApiClient.GetShowCastingAsync(show);
                    if (castingResult.IsSuccess)
                    {
                        await ProcessCastingAsync(castingResult.Data.ToList());
                        scope.Complete();
                    }
                }
            }
        }, _configuration.GetValue<int>("MaxDegreeOfParallelism:ShowsEndPoint"));
    }

    private async Task ProcessCastingAsync(List<Cast> casts)
    {
        //optimizing a bit the use of the connection, as it should be a quick batch and the DB is local
        //using (var cnn = _showService.CreateSession())
        //{
        await casts.AsyncParallelForEach(async cast =>
        {
            await _showService.TryAddCastAsync(cast);//, cnn);
        }, _configuration.GetValue<int>("MaxDegreeOfParallelism:CastingEndPoint"));
        //}
    }

    private async Task<int> GetApiPageAsync()
    {
        var lastShow = await _showService.LastShowAsync();
        return lastShow == null
            ? 0
            : Convert.ToInt32(Math.Floor(lastShow.ExternalId / 250M));
    }
}
