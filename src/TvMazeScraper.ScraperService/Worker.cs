using Dapper;
using System.Data;
using System.Threading.Tasks.Dataflow;
using System.Transactions;
using TvMazeScraper.Application.Interfaces;
using TvMazeScraper.Domain.Entities;
using TvMazeScraper.Infrastructure.Database;

namespace TvMazeScraper.ScraperService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ITVShowScraper _tvShowScraper;

    public Worker(
        ILogger<Worker> logger,
        ITVShowScraper tvShowScraper)
    {
        _logger = logger;
        _tvShowScraper = tvShowScraper;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _tvShowScraper.StartAsync(stoppingToken);
        _logger.LogInformation("Done! We are finished.");
    }
}






