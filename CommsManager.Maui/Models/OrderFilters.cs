namespace CommsManager.Maui.Models;

public class OrderFilter
{
    public string? Status { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? ArtistId { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public DateTime? DeadlineFrom { get; set; }
    public DateTime? DeadlineTo { get; set; }
}

