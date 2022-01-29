using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.Data;
using TvMazeScraper.Application.Interfaces;

namespace TvMazeScraper.Infrastructure.Database;

public class SQLiteDatabaseBootstrapper : IDatabaseBootstrapper
{
    private const string TABLE_SHOWS_NAME = "Shows";
    private const string TABLE_CASTING_NAME = "Casting";

    private readonly IConfiguration _configuration;
    private readonly IDbContext _dbContext;

    public SQLiteDatabaseBootstrapper(
        IConfiguration configuration,
        IDbContext dbContext)
    {
        _configuration = configuration;
        _dbContext = dbContext;
    }

    public async Task SetupAsync()
    {
        using (var connection = _dbContext.CreateConnection())
        {
            connection.Open();
            await CreateTableShowsAsync(connection);
            await CreateTableCastingsAsync(connection);
        }
    }

    private async Task CreateTableCastingsAsync(IDbConnection connection)
    {
        var table = await connection.QueryAsync<string>(@$"
SELECT name 
    FROM sqlite_master 
    WHERE type='table' 
        AND name = '{TABLE_CASTING_NAME}'
;");

        var tableName = table.FirstOrDefault();
        if (!string.IsNullOrEmpty(tableName) && tableName == TABLE_CASTING_NAME)
            return;

        await connection.ExecuteAsync(@$"
CREATE TABLE {TABLE_CASTING_NAME} (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UniqueId TEXT NOT NULL,
    ExternalId INTEGER NOT NULL,
    ShowId INTEGER NOT NULL,
    Name VARCHAR(100) NOT NULL,
    Birthday TEXT
);");

        await connection.ExecuteAsync(@$"
CREATE INDEX 
    IX_Birthday 
    ON {TABLE_CASTING_NAME}
        (Birthday DESC)
;");
    }

    private async Task CreateTableShowsAsync(IDbConnection connection)
    {
        var table = await connection.QueryAsync<string>(@$"
SELECT name 
    FROM sqlite_master 
    WHERE type='table' 
        AND name = '{TABLE_SHOWS_NAME}'
;");

        var tableName = table.FirstOrDefault();
        if (!string.IsNullOrEmpty(tableName) && tableName == TABLE_SHOWS_NAME)
            return;

        await connection.ExecuteAsync(@$"
CREATE TABLE {TABLE_SHOWS_NAME} (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UniqueId TEXT NOT NULL,
    ExternalId INTEGER NOT NULL,
    Name VARCHAR(100) NOT NULL
);");
    }
}
