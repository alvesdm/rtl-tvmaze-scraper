using Dapper;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using TvMazeScraper.Application.Interfaces;
using TvMazeScraper.Infrastructure.Database;

namespace TvMazeScraper.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IDbContext, SQLiteDapperContext>();
        services.AddTransient<IDatabaseBootstrapper, SQLiteDatabaseBootstrapper>();

        SqlMapper.AddTypeHandler(new MySqlGuidTypeHandler());
        SqlMapper.RemoveTypeMap(typeof(Guid));
        SqlMapper.RemoveTypeMap(typeof(Guid?));

        return services;
    }


}

public class MySqlGuidTypeHandler : SqlMapper.TypeHandler<Guid>
{
    public override void SetValue(IDbDataParameter parameter, Guid guid)
    {
        parameter.Value = guid.ToString();
    }

    public override Guid Parse(object value)
    {
        return new Guid((string)value);
    }
}