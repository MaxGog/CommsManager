namespace CommsManager.Core.Models;

public class Commission
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public List<byte[]>? ViewAttachment { get; set; }
    public string? TypeCommission { get; set; }
    public string? Price { get; set; }
}