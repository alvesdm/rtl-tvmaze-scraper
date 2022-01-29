using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Domain.Entities;

namespace TvMazeScraper.Application.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Cast> Casts { get; }
    public DbSet<Show> Shows { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
