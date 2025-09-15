namespace CommsManager.Models;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = "#007bff";
}

public class CommissionTag
{
    public int CommissionId { get; set; }
    public Commission Commission { get; set; } = null!;

    public int TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}

public class PortfolioTag
{
    public int PortfolioItemId { get; set; }
    public PortfolioItem PortfolioItem { get; set; } = null!;
    
    public int TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}