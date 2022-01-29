namespace TvMazeScraper.Domain.Entities;

public class Show : IEntity<int>
{
    public int Id { get; set; }
    public Guid UniqueId { get; set; }
    public int ExternalId { get; set; }
    public string Name { get; set; }

    public virtual List<Cast> Casting { get; set; }
}
