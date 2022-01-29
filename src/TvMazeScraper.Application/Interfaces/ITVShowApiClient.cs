using TvMazeScraper.Domain.Entities;

namespace TvMazeScraper.Application.Interfaces;

public interface ITVShowApiClient
{
    //https://api.tvmaze.com/shows?page=0
    Task<Result<IEnumerable<Cast>>> GetShowCastingAsync(Show show);

    //https://api.tvmaze.com/shows/1/cast
    Task<Result<IEnumerable<Show>>> GetShowsPageAsync(int apiPage);
}
