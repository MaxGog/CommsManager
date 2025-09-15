using System.ComponentModel.DataAnnotations;

namespace CommsManager.Models;

public class Customer
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [EmailAddress]
    [MaxLength(150)]
    public string? Email { get; set; }
    
    [Phone]
    [MaxLength(20)]
    public string? Phone { get; set; }
    
    [MaxLength(200)]
    public string? SocialMedia { get; set; }
    
    [MaxLength(500)]
    public string? Notes { get; set; }
    
    public string? ContactId { get; set; }
    
    public List<Commission> Commissions { get; set; } = new List<Commission>();
}