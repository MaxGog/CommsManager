using System.ComponentModel.DataAnnotations;
using CommsManager.Models.Enums;

namespace CommsManager.Models;

public class Commission
{
    public Commission()
    {
        CreatedDate = DateTime.UtcNow;
        Title = "Новая комиссия";
        Status = CommissionStatus.Queued;
        Price = 0;
        ArtType = ArtType.Other;
        Priority = 1;
        IsPaid = false;
        Tags = new List<CommissionTag>();
        Artworks = new List<Artwork>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public CommissionStatus Status { get; set; }

    [Required]
    public ArtType ArtType { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? Deadline { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(0, double.MaxValue)]
    public decimal AdvancePayment { get; set; }

    public bool IsPaid { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public List<Artwork> Artworks { get; set; } = new List<Artwork>();
    public List<CommissionTag> Tags { get; set; } = new List<CommissionTag>();

    public string? SketchPath { get; set; }
    public string? FinalArtPath { get; set; }

    public string? Notes { get; set; }
    public int Priority { get; set; } = 0;
}
