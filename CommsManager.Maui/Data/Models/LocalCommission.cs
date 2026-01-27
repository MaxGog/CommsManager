using SQLite;
using System.Text.Json;

namespace CommsManager.Maui.Data.Models;

[Table("Commissions")]
public class LocalCommission
{
    [PrimaryKey]
    public Guid Id { get; set; }

    [NotNull]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? TypeCommission { get; set; }

    public string? Price { get; set; }

    [Indexed]
    public Guid ArtistProfileId { get; set; }

    public string? ViewAttachmentJson { get; set; }

    [Ignore]
    public List<byte[]>? ViewAttachment
    {
        get => string.IsNullOrEmpty(ViewAttachmentJson)
            ? null
            : JsonSerializer.Deserialize<List<byte[]>>(ViewAttachmentJson);
        set => ViewAttachmentJson = JsonSerializer.Serialize(value);
    }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }
}