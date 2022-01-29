using System.Data;

namespace TvMazeScraper.Application.Interfaces;

public interface IDBConnectable
{
    IDbConnection CreateConnection(bool open = true);
}
