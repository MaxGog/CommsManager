using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommsManager.Models;
public class MaterialInventory
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    public string? Type { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? Quantity { get; set; }

    [StringLength(20)]
    public string? Unit { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? PricePerUnit { get; set; }

    [StringLength(200)]
    public string? Supplier { get; set; }

    public DateTime LastRestock { get; set; } = DateTime.UtcNow;

    public int? LowStockThreshold { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    [ForeignKey("UserId")]
    public virtual ApplicationUser User { get; set; } = null!;
}
