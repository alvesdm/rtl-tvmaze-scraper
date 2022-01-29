namespace TvMazeScraper.Application.Api.Models;

public struct TVMazeCastModel
{
    public TVMazePersonModel Person { get; set; }
}

public struct CastModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Birthday { get; set; }
}