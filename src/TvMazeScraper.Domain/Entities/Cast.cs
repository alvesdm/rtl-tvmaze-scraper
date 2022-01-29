using System.ComponentModel.DataAnnotations.Schema;

namespace TvMazeScraper.Domain.Entities;

[Table("Casting")]
public class Cast : IEntity<int>
{
    public int Id { get; set; }
    public Guid UniqueId { get; set; }
    public int ExternalId { get; set; }
    public string Name { get; set; }
    public DateTime? Birthday { get; set; }

    public int ShowId { get; set; }
    [ForeignKey(nameof(ShowId))]
    public virtual Show Show { get; set; }
}
