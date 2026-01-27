using SQLite;

namespace CommsManager.Maui.Data.Models;

[Table("Emails")]
public class LocalEmail
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull]
    public string EmailAdress { get; set; } = string.Empty;

    public string? TypeEmail { get; set; }
    public string? Description { get; set; }

    [Indexed]
    public Guid? CustomerId { get; set; }

    [Indexed]
    public Guid? ArtistProfileId { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}