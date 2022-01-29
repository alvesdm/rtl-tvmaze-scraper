using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using TvMazeScraper.Application.Constants;
using TvMazeScraper.Application.Interfaces;
using TvMazeScraper.Domain.Entities;

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

public class EFContext : DbContext, IApplicationDbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<Cast> Casts => Set<Cast>();
    public DbSet<Show> Shows => Set<Show>();
    public EFContext(
        IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlite(_configuration.GetConnectionString(ConfigurationConstants.CONNECTIONSTRINGS_DEFAULTCONNECTION));
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}