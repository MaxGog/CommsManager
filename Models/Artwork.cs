namespace CommsManager.Models;

public class Artwork
{
    public int Id { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public DateTime UploadDate { get; set; } = DateTime.UtcNow;
    public bool IsFinal { get; set; }
    public string? Description { get; set; }
    
    public int CommissionId { get; set; }
    public Commission Commission { get; set; } = null!;
}