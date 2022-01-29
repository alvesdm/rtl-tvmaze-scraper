using TvMazeScraper.Domain.Entities;

namespace TvMazeScraper.Application.Interfaces;

public interface IShowService
{
    Task<IEnumerable<Show>> GetShows(int pageSize, int page);
}
