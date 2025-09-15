using System.ComponentModel.DataAnnotations;
using CommsManager.Models.Enums;

namespace CommsManager.Models;

public class PricePreset
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public ArtType ArtType { get; set; }
    
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    
    [Range(0, double.MaxValue)]
    public decimal BasePrice { get; set; }
    
    public decimal AdditionalCharacterPrice { get; set; }
    public decimal BackgroundPrice { get; set; }
    public decimal ComplexBackgroundPrice { get; set; }
    public decimal RushOrderPrice { get; set; }
    
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
    
    public int EstimatedDays { get; set; } = 7;
}