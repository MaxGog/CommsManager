public class Order
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime Deadline { get; set; }
    public OrderStatus Status { get; set; }

    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }

    public ICollection<OrderAttachment> Attachments { get; set; }
}

public enum OrderStatus
{
    New,
    InProgress,
    Completed,
    Cancelled
}