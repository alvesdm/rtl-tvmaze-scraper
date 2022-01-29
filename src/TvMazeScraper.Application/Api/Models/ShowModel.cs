namespace TvMazeScraper.Application.Api.Models;

public struct TVMazeShowModel
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public struct ShowModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<CastModel> Cast { get; set; }
}
