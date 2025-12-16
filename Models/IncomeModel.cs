using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommsManager.Models;
public class Income
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = null!;

    public int? OrderId { get; set; }

    [Required]
    [StringLength(100)]
    public string Source { get; set; } = null!;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public bool IsRecurring { get; set; } = false;

    [ForeignKey("UserId")]
    public virtual ApplicationUser User { get; set; } = null!;

    [ForeignKey("OrderId")]
    public virtual Order? Order { get; set; }
}

public enum PaymentMethod
{
    Cash,
    BankTransfer,
    PayPal,
    YooMoney,
    Sberbank,
    Tinkoff,
    Other
}
