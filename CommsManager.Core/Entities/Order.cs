using CommsManager.Core.Enums;
using CommsManager.Core.ValueObjects;

namespace CommsManager.Core.Entities;

public class Order : BaseEntity
{
    private Order() { } // Для EF Core

    public Order(
        string title,
        string description,
        Money price,
        Guid customerId,
        DateTime deadline)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        Price = price;
        CustomerId = customerId;
        Deadline = deadline;
        Status = OrderStatus.New;
        CreatedDate = DateTime.UtcNow;
        IsActive = true;

        AddDomainEvent(new OrderCreatedEvent(this));
    }

    public string Title { get; private set; }
    public string Description { get; private set; }
    public Money Price { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime Deadline { get; private set; }
    public OrderStatus Status { get; private set; }
    public bool IsActive { get; private set; }

    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; }

    public Guid? ArtistProfileId { get; private set; }
    public ArtistProfile ArtistProfile { get; private set; }

    private readonly List<OrderAttachment> _attachments = new();
    public IReadOnlyCollection<OrderAttachment> Attachments => _attachments.AsReadOnly();

    private readonly List<OrderNote> _notes = new();
    public IReadOnlyCollection<OrderNote> Notes => _notes.AsReadOnly();

    public void UpdateStatus(OrderStatus newStatus)
    {
        if (Status == OrderStatus.Completed || Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Невозможно изменить статус выполненного или отмененного заказа");

        Status = newStatus;

        AddDomainEvent(new OrderStatusChangedEvent(Id, Status, DateTime.UtcNow));
    }

    public void AddAttachment(string fileName, string fileUrl, string contentType)
    {
        var attachment = new OrderAttachment(fileName, fileUrl, contentType);
        _attachments.Add(attachment);
    }

    public void AddNote(string content, Guid createdBy)
    {
        var note = new OrderNote(content, createdBy);
        _notes.Add(note);
    }

    public void UpdatePrice(Money newPrice)
    {
        if (Status == OrderStatus.Completed)
            throw new InvalidOperationException("Невозможно изменить цену выполненного заказа");

        Price = newPrice;
    }

    public void Cancel(string reason)
    {
        Status = OrderStatus.Cancelled;
        IsActive = false;

        AddDomainEvent(new OrderCancelledEvent(Id, reason, DateTime.UtcNow));
    }
}