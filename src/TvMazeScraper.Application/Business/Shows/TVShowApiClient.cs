using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TvMazeScraper.Application.Api.Models;
using TvMazeScraper.Application.Constants;
using TvMazeScraper.Application.Interfaces;
using TvMazeScraper.Domain.Entities;

namespace TvMazeScraper.Application.Business.Shows;

public class TVShowApiClient : ITVShowApiClient
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public TVShowApiClient(
        IConfiguration configuration,
        HttpClient httpClient)
    {
        httpClient.BaseAddress = new Uri(configuration.GetValue<string>(ConfigurationConstants.TVMAZEAPI_BASEURI));

        _configuration = configuration;
        _httpClient = httpClient;
    }



    //https://api.tvmaze.com/shows/1/cast
    public async Task<Result<IEnumerable<Cast>>> GetShowCastingAsync(Show show)
    {
        var response = await _httpClient.SendAsync(
            new HttpRequestMessage(HttpMethod.Get, $"/shows/{show.ExternalId}/cast"));

        if (!response.IsSuccessStatusCode)
        {
            return new Result<IEnumerable<Cast>>(new Exception("Opssss! TODO"));
        }

        var persons = JsonConvert.DeserializeObject<List<TVMazeCastModel>>(await response.Content.ReadAsStringAsync());


        return new Result<IEnumerable<Cast>>(persons.Select(p => new Cast
        {
            ExternalId = p.Person.Id,
            Name = p.Person.Name,
            Birthday = p.Person.Birthday,
            ShowId = show.Id
        }));
    }

    //https://api.tvmaze.com/shows?page=0
    public async Task<Result<IEnumerable<Show>>> GetShowsPageAsync(int apiPage)
    {
        var response = await _httpClient.SendAsync(
            new HttpRequestMessage(HttpMethod.Get, $"/shows?page={apiPage}"));

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new Result<IEnumerable<Show>>(new Exception("No such page.We might have finished our scraping process."));
            }

            return new Result<IEnumerable<Show>>(new Exception("Opssss! Something went real bad."));

            ///Error 429, is handled by polly
        }

        var shows = JsonConvert.DeserializeObject<List<TVMazeShowModel>>(await response.Content.ReadAsStringAsync());

        return new Result<IEnumerable<Show>>(shows.Select(p => new Show
        {
            ExternalId = p.Id,
            Name = p.Name
        }));
    }
}
