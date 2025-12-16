using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommsManager.Models;
public class Expense
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Category { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(100)]
    public string? Vendor { get; set; }

    public bool IsBusinessExpense { get; set; } = true;

    public int? OrderId { get; set; }

    [ForeignKey("UserId")]
    public virtual ApplicationUser User { get; set; } = null!;

    [ForeignKey("OrderId")]
    public virtual Order? Order { get; set; }
}
