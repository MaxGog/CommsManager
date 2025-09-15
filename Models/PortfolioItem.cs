using System.ComponentModel.DataAnnotations;
using CommsManager.Models.Enums;

namespace CommsManager.Models;

public class PortfolioItem
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    public ArtType ArtType { get; set; }
    
    [Required]
    public string ImagePath { get; set; } = string.Empty;
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public bool IsFeatured { get; set; }
    
    [Range(0, double.MaxValue)]
    public decimal? Price { get; set; }
    
    public List<PortfolioTag> Tags { get; set; } = new List<PortfolioTag>();
    
    public int? CommissionId { get; set; }
    public Commission? Commission { get; set; }
    
    public string? CharacterName { get; set; }
    public string? Style { get; set; }
    public int HoursSpent { get; set; }
}
