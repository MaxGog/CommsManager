using System.Text.Json;
using SQLite;

namespace CommsManager.Maui.Data.Models;

[Table("ArtistProfiles")]
public class LocalArtistProfile
{
    [PrimaryKey]
    public Guid Id { get; set; }

    [NotNull]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Ignore]
    public byte[]? ArtistPicture { get; set; }
    public string? ArtistPicturePath { get; set; }

    [Ignore]
    public byte[]? ArtistBanner { get; set; }
    public string? ArtistBannerPath { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? PhonesJson { get; set; }
    public string? EmailsJson { get; set; }
    public string? SocialLinksJson { get; set; }
    public string? CommissionsJson { get; set; }

}