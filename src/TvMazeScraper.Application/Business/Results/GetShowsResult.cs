using TvMazeScraper.Application.Api.Models;

namespace TvMazeScraper.Application.Business.Results;

public class GetShowsResult
{
    public IEnumerable<ShowModel> Shows { get; set; }
}

