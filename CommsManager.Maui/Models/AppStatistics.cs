namespace CommsManager.Maui.Models;

public class AppStatistics
{
    public int TotalCustomers { get; set; }
    public int ActiveCustomers { get; set; }
    public int TotalOrders { get; set; }
    public int ActiveOrders { get; set; }
    public int TotalArtists { get; set; }
    public int TotalCommissions { get; set; }
    public int NewOrders { get; set; }
    public int InProgressOrders { get; set; }
    public int CompletedOrders { get; set; }
    public int OverdueOrders { get; set; }
    public decimal TotalRevenue { get; set; }
}