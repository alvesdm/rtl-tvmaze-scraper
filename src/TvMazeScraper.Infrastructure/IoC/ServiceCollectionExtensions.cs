using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TvMazeScraper.Application.Interfaces;
using TvMazeScraper.Infrastructure.Database;

namespace TvMazeScraper.Infrastructure.IoC;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, bool isConsoleApp = false)
    {
        if (!isConsoleApp)
        {
            services.AddDbContext<EFContext>(options =>
            {
                //var config = options.GetRequiredService<IConfiguration>();
                ////options.UseSqlite(config.GetConnectionString(ConfigurationConstants.CONNECTIONSTRINGS_DEFAULTCONNECTION));
                options.UseSqlite("Data Source=..\\Database\\TVMazeShows.db;");
            });
            //services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<EFContext>());
            services.AddScoped<IApplicationDbContext, EFContext>();
        }
        else
        {
            services.AddTransient<IDatabaseBootstrapper, SQLiteDatabaseBootstrapper>();
            services.AddTransient<IDbContext, SQLiteDapperContext>();
            SqlMapper.AddTypeHandler(new SQLiteGuidTypeHandler());
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));
        }

        return services;
    }
}
