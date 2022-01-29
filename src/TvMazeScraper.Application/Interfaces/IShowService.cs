using System.Data;
using TvMazeScraper.Domain.Entities;

namespace TvMazeScraper.Application.Interfaces;

public interface IShowService
{
    IDbConnection CreateSession();
    Task<bool> TryAddShowAsync(Show show, IDbConnection cnn = default);
    Task<Show> LastShowAsync(IDbConnection cnn = default);
    Task<bool> TryAddCastAsync(Cast cast, IDbConnection cnn = default);
}
