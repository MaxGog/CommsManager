using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommsManager.Models;

public class Service
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal BasePrice { get; set; }

    public ServiceType Type { get; set; }

    public bool IsActive { get; set; } = true;

    public int? EstimatedCompletionDays { get; set; }

    [StringLength(1000)]
    public string? Requirements { get; set; }

    [ForeignKey("UserId")]
    public virtual ApplicationUser User { get; set; } = null!;
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

public enum ServiceType
{
    DigitalArt,
    TraditionalArt,
    Fursuit,
    Cosplay,
    Craft,
    Commission,
    Other
}