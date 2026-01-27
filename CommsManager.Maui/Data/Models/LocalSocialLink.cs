using SQLite;

namespace CommsManager.Maui.Data.Models;

[Table("SocialLinks")]
public class LocalSocialLink
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull]
    public string Link { get; set; } = string.Empty;

    public string? TypeLink { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsVisible { get; set; } = true;

    [Indexed]
    public Guid? CustomerId { get; set; }

    [Indexed]
    public Guid? ArtistProfileId { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}