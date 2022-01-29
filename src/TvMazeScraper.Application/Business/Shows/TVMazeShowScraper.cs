using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Transactions;
using TvMazeScraper.Application.Constants;
using TvMazeScraper.Application.Extensions;
using TvMazeScraper.Application.Interfaces;
using TvMazeScraper.Domain.Entities;

namespace TvMazeScraper.Application.Business.Shows;

public class TVMazeShowScraper : ITVShowScraper
{
    private readonly ILogger<TVMazeShowScraper> _logger;
    private readonly IConfiguration _configuration;
    private readonly ITVShowApiClient _tvShowApiClient;
    private readonly IScraperRepository _showService;

    public TVMazeShowScraper(
        ILogger<TVMazeShowScraper> logger,
        IConfiguration configuration,
        ITVShowApiClient tvShowApiClient,
        IScraperRepository showService)
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
        }, _configuration.GetValue<int>(ConfigurationConstants.MAXDEGREEOFPARALLELISM_SHOWSENDPOINT));
    }

    private async Task ProcessCastingAsync(List<Cast> casts)
    {
        await casts.AsyncParallelForEach(async cast =>
        {
            await _showService.TryAddCastAsync(cast);
        }, _configuration.GetValue<int>(ConfigurationConstants.MAXDEGREEOFPARALLELISM_CASTINGENDPOINT));
    }

    private async Task<int> GetApiPageAsync()
    {
        var lastShow = await _showService.LastShowAsync();
        return lastShow == null
            ? 0
            : Convert.ToInt32(Math.Floor(lastShow.ExternalId / _configuration.GetValue<decimal>(ConfigurationConstants.TVMAZEAPI_SHOWSPAGESIZE)));
    }
}
