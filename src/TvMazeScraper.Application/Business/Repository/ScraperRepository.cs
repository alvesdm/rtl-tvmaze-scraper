﻿using Dapper;
using System.Data;
using TvMazeScraper.Application.Constants;
using TvMazeScraper.Application.Interfaces;
using TvMazeScraper.Domain.Entities;

namespace TvMazeScraper.Application.Business.Repository;
/// <summary>
/// EF doesnt do well with parallel tasks, that's when Dapper comes in handy
/// </summary>
public class ScraperRepository : IScraperRepository
{
    private readonly IDbContext _dbContext;

    public ScraperRepository(
        IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Show> LastShowAsync()
    {
        using (var connection = _dbContext.CreateConnection())
        {
            var show = await connection.QueryFirstOrDefaultAsync<Show>(@$"
SELECT 
    * 
    FROM {DatabaseConstants.TABLE_SHOWS_NAME} 
    ORDER BY {nameof(Show.ExternalId)} DESC
    LIMIT 1
;");
            return show;
        }
    }

    public async Task<bool> TryAddCastAsync(Cast cast)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            var count = await connection.QueryFirstAsync<int>(@$"
SELECT 
    COUNT(0) 
    FROM {DatabaseConstants.TABLE_CASTING_NAME} 
    WHERE {nameof(Cast.ExternalId)} = {cast.ExternalId}
;");
            if (count > 0)
            {
                return false;
            }

            var id = await connection.ExecuteScalarAsync<int>(@$"
INSERT 
    INTO {DatabaseConstants.TABLE_CASTING_NAME} 
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
                cast.ExternalId,
                cast.ShowId,
                cast.Name,
                cast.Birthday
            });

            cast.Id = id;
            return true;
        }
    }

    public async Task<bool> TryAddShowAsync(Show show)
    {
        using (var connection = _dbContext.CreateConnection())
        {
            var count = await connection.QueryFirstAsync<int>(@$"
SELECT 
    COUNT(0) 
    FROM {DatabaseConstants.TABLE_SHOWS_NAME} 
    WHERE {nameof(Cast.ExternalId)} = {show.ExternalId}
;");
            if (count > 0)
            {
                return false;
            }

            var id = await connection.ExecuteScalarAsync<int>(@$"
INSERT 
    INTO {DatabaseConstants.TABLE_SHOWS_NAME} 
    ({nameof(Show.UniqueId)},
    {nameof(Show.ExternalId)},
    {nameof(Show.Name)})
    VALUES 
    (@UniqueId, @ExternalId, @Name)
;
select last_insert_rowid();", new
            {
                UniqueId = Guid.NewGuid(),
                show.ExternalId,
                show.Name
            });

            show.Id = id;

            return true;
        }
    }
}
