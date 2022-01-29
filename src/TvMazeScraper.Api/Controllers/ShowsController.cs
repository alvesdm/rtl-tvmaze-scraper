using Microsoft.AspNetCore.Mvc;

namespace TvMazeScraper.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShowsController : ControllerBase
    {
        private readonly ILogger<ShowsController> _logger;

        public ShowsController(ILogger<ShowsController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "Shows")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}