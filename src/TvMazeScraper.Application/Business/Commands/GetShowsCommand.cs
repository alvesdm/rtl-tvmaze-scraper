using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TvMazeScraper.Application.Business.Results;
using TvMazeScraper.Application.Extensions;
using TvMazeScraper.Application.Interfaces;

/// <summary>
/// I like keeping the Command and the CommandHandler together 
/// as it provides a better visibility as well as quick access.
/// </summary>
namespace TvMazeScraper.Application.Business.Commands;

public class GetShowsCommand : IRequest<GetShowsResult>
{
    public int PageSize { get; set; }
    public int Page { get; set; }
}

public class CalculateFraudCommandHandler : IRequestHandler<GetShowsCommand, GetShowsResult>
{
    private readonly ILogger<CalculateFraudCommandHandler> _logger;
    private readonly IShowService _showService;

    public CalculateFraudCommandHandler(
        ILogger<CalculateFraudCommandHandler> logger,
        IShowService showService)
    {
        _logger = logger;
        _showService = showService;
    }

    public async Task<GetShowsResult> Handle(GetShowsCommand request, CancellationToken cancellationToken)
    {
        var pageSize = request.PageSize < 1 ? 10 : request.PageSize;
        var page = request.Page < 1 ? 1 : request.Page;
        var shows = await _showService.GetShows(pageSize, page);

        return new GetShowsResult
        {
            Shows = shows.Select(s => new Api.Models.ShowModel
            {
                Id = s.ExternalId,
                Name = s.Name,
                Cast = s.Casting.Select(c => new Api.Models.CastModel
                {
                    Id = c.ExternalId,
                    Birthday = c.Birthday.TryFormatDate("yyyy-MM-dd"),
                    Name = c.Name
                }).OrderByDescending(c=>c.Birthday)
            })
        };
    }
}
