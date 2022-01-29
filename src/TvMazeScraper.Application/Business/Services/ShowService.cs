using Microsoft.EntityFrameworkCore;
using System.Data;
using TvMazeScraper.Application.Interfaces;
using TvMazeScraper.Domain.Entities;

namespace TvMazeScraper.Application.Business.Services;

public class ShowService : IShowService
{
    private readonly IApplicationDbContext _context;

    public ShowService(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Show>> GetShows(int pageSize, int page)
    {
        return
            _context
            .Shows
            .OrderBy(s => s.ExternalId)
            .Skip(pageSize * (page - 1))
            .Take(pageSize)
            .Include(c => c.Casting)
            .ToList();
    }
}