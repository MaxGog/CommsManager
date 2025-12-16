using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommsManager.Models;
public class Client
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [EmailAddress]
    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(20)]
    public string? Phone { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    [StringLength(100)]
    public string? ContactPlatform { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? LastContactDate { get; set; }

    public ClientStatus Status { get; set; } = ClientStatus.Active;

    [ForeignKey("UserId")]
    public virtual ApplicationUser User { get; set; } = null!;
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

public enum ClientStatus
{
    Active,
    Inactive,
    VIP,
    Blacklisted
}
