using SQLite;
using System.Text.Json;

namespace CommsManager.Maui.Data.Models;

[Table("Orders")]
public class LocalOrder
{
    [PrimaryKey]
    public Guid Id { get; set; }

    [NotNull, Indexed]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    [NotNull]
    public decimal PriceAmount { get; set; }

    [NotNull, MaxLength(3)]
    public string PriceCurrency { get; set; } = "USD";

    [MaxLength(5)]
    public string? PriceSymbol { get; set; }

    [NotNull]
    public DateTime CreatedDate { get; set; }

    [NotNull]
    public DateTime Deadline { get; set; }

    [NotNull, MaxLength(50), Indexed]
    public string Status { get; set; } = "New";

    [NotNull]
    public bool IsActive { get; set; } = true;

    [NotNull, Indexed]
    public Guid CustomerId { get; set; }

    [NotNull, Indexed]
    public Guid ArtistId { get; set; }

    [Ignore]
    public LocalCustomer? Customer { get; set; }

    [Ignore]
    public LocalArtistProfile? Artist { get; set; }

    [Ignore]
    public bool IsOverdue => Deadline < DateTime.UtcNow &&
                            Status != "Completed" &&
                            Status != "Cancelled" &&
                            IsActive;

    [Ignore]
    public TimeSpan TimeUntilDeadline => Deadline - DateTime.UtcNow;

    [Ignore]
    public bool IsDueSoon => TimeUntilDeadline.TotalDays <= 3 &&
                            TimeUntilDeadline.TotalDays > 0 &&
                            Status != "Completed" &&
                            Status != "Cancelled" &&
                            IsActive;

    public void Complete()
    {
        Status = "Completed";
        IsActive = false;
    }

    public void Cancel()
    {
        Status = "Cancelled";
        IsActive = false;
    }

    public void MarkAsInProgress()
    {
        Status = "InProgress";
    }
}