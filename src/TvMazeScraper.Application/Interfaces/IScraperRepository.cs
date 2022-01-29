using System.Data;
using TvMazeScraper.Domain.Entities;

namespace TvMazeScraper.Application.Interfaces;

public interface IScraperRepository
{
    Task<bool> TryAddShowAsync(Show show);
    Task<Show> LastShowAsync();
    Task<bool> TryAddCastAsync(Cast cast);
}
