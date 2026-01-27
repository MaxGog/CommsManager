using SQLite;

namespace CommsManager.Maui.Data.Models;

[Table("Phones")]
public class LocalPhone
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull]
    public string NumberPhone { get; set; } = string.Empty;

    public string? TypePhone { get; set; }
    public string? RegionNumber { get; set; }
    public string? Description { get; set; }

    [Indexed]
    public Guid? CustomerId { get; set; }

    [Indexed]
    public Guid? ArtistProfileId { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}