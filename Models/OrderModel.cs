using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommsManager.Models;
public class Order
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = null!;

    [Required]
    public int ClientId { get; set; }

    [Required]
    public int ServiceId { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = null!;

    [StringLength(2000)]
    public string? Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Deposit { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public DateTime? Deadline { get; set; }

    public DateTime? CompletionDate { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Received;

    [StringLength(1000)]
    public string? Notes { get; set; }

    [StringLength(500)]
    public string? ReferenceLinks { get; set; }

    public bool IsPaid { get; set; } = false;

    [ForeignKey("UserId")]
    public virtual ApplicationUser User { get; set; } = null!;

    [ForeignKey("ClientId")]
    public virtual Client Client { get; set; } = null!;

    [ForeignKey("ServiceId")]
    public virtual Service Service { get; set; } = null!;
}

public enum OrderStatus
{
    Received,
    InProgress,
    ReadyForReview,
    Revisions,
    Completed,
    Cancelled
}