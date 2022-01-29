using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.Data;
using TvMazeScraper.Application.Constants;
using TvMazeScraper.Application.Interfaces;

namespace TvMazeScraper.Infrastructure.Database;

public class SQLiteDapperContext : IDbContext
{
    private readonly IConfiguration _configuration;

    public SQLiteDapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection CreateConnection(bool open = true) {
        var cnn = new SqliteConnection(_configuration.GetConnectionString(ConfigurationConstants.CONNECTIONSTRINGS_DEFAULTCONNECTION));
        if(open)
            cnn.Open();

        return cnn;
    }
}