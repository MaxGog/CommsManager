namespace CommsManager.Models;

public class AnalyticsRecord
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public int CompletedCommissions { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal AverageCommissionPrice { get; set; }
    
    public int SketchCount { get; set; }
    public int FullBodyCount { get; set; }
    public int SceneCount { get; set; }
    //TODO: Дописать для аналитики
}
