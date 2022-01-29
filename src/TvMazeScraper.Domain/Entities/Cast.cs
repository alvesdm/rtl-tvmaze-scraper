namespace TvMazeScraper.Domain.Entities;

public class Cast : IEntity<int>
{
    public int Id { get; set; }
    public Guid UniqueId { get; set; }
    public int ExternalId { get; set; }
    public int ShowId { get; set; }
    public string Name { get; set; }
    public DateTime? Birthday { get; set; }
}
