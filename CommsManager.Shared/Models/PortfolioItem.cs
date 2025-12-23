namespace CommsManager.Shared.Models;

public class PortfolioItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public List<string> Tags { get; set; } = new();
}