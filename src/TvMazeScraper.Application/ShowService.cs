using Dapper;
using System.Data;
using TvMazeScraper.Application.Interfaces;
using TvMazeScraper.Domain.Entities;

namespace TvMazeScraper.Application;

public class ShowService : IShowService
{
    private const string TABLE_SHOWS_NAME = "Shows";
    private const string TABLE_CASTING_NAME = "Casting";

    private readonly IDbContext _dbContext;

    public ShowService(
        IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IDbConnection CreateSession()
    {
        return _dbContext.CreateConnection();
    }

    public async Task<Show> LastShowAsync(IDbConnection cnn = default)
    {
        using (var connection = cnn ?? _dbContext.CreateConnection())
        {
            var show = await connection.QueryFirstOrDefaultAsync<Show>(@$"
SELECT 
    * 
    FROM {TABLE_SHOWS_NAME} 
    ORDER BY {nameof(Show.Id)} DESC
    LIMIT 1
;");
            return show;
        }
    }

    public async Task<bool> TryAddCastAsync(Cast cast, IDbConnection cnn = default)
    {
        using (var connection = cnn ?? _dbContext.CreateConnection())
        {
            var count = await connection.QueryFirstAsync<int>(@$"
SELECT 
    COUNT(0) 
    FROM {TABLE_CASTING_NAME} 
    WHERE {nameof(Cast.ExternalId)} = {cast.ExternalId}
;");
            if (count > 0)
            {
                return false;
            }

            var id = await connection.ExecuteScalarAsync<int>(@$"
INSERT 
    INTO {TABLE_CASTING_NAME} 
    ({nameof(Cast.UniqueId)},
    {nameof(Cast.ExternalId)},
    {nameof(Cast.ShowId)},
    {nameof(Cast.Name)},
    {nameof(Cast.Birthday)})
    VALUES 
    (@UniqueId,@ExternalId,@ShowId,@Name,@Birthday)
;
select last_insert_rowid();", new
            {
                UniqueId = Guid.NewGuid(),
                ExternalId = cast.ExternalId,
                ShowId = cast.ShowId,
                Name = cast.Name,
                Birthday = cast.Birthday
            });

            cast.Id = id;
            return true;
        }
    }

    public async Task<bool> TryAddShowAsync(Show show, IDbConnection cnn = default)
    {
        using (var connection = cnn ?? _dbContext.CreateConnection())
        {
            var count = await connection.QueryFirstAsync<int>(@$"
SELECT 
    COUNT(0) 
    FROM {TABLE_SHOWS_NAME} 
    WHERE {nameof(Cast.ExternalId)} = {show.ExternalId}
;");
            if (count > 0)
            {
                return false;
            }

            var id = await connection.ExecuteScalarAsync<int>(@$"
INSERT 
    INTO {TABLE_SHOWS_NAME} 
    ({nameof(Show.UniqueId)},
    {nameof(Show.ExternalId)},
    {nameof(Show.Name)})
    VALUES 
    (@UniqueId, @ExternalId, @Name)
;
select last_insert_rowid();", new
            {
                UniqueId = Guid.NewGuid(),
                ExternalId = show.ExternalId,
                Name = show.Name
            });

            show.Id = id;

            return true;
        }
    }
}
