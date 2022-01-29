using Microsoft.AspNetCore.Mvc;
using TvMazeScraper.Application.Api.Models;
using TvMazeScraper.Application.Business.Commands;

namespace TvMazeScraper.Api.Controllers
{
    public class ShowsController : ApiControllerBase
    {
        [HttpGet]
        [Route("{PageSize:int}/{Page:int}")]
        public async Task<IActionResult> Get([FromRoute] GetShowsCommand command)
        {
            var showsResult = await Mediator.Send(command);
            return Ok(showsResult.Shows);
        }
    }
}