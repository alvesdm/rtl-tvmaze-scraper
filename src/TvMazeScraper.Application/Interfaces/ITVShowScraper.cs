namespace TvMazeScraper.Application.Interfaces;

public interface ITVShowScraper
{
    Task StartAsync(CancellationToken cancellationToken);
}
