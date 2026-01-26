using CommsManager.Core.Enums;
using CommsManager.Core.Events;
using CommsManager.Core.ValueObjects;
using CommsManager.Core.Models;

namespace CommsManager.Core.Entities;

public class Order : BaseEntity
{
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public Money Price { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime Deadline { get; private set; }
    public OrderStatus Status { get; private set; }
    public bool IsActive { get; private set; }

    public Guid CustomerId { get; private set; }
    public Guid ArtistId { get; private set; }

    private readonly List<OrderAttachment> _attachments = [];
    public IReadOnlyCollection<OrderAttachment> Attachments => _attachments.AsReadOnly();
    public void AddAttachment(string fileName, byte[] file, AttachmentType contentType)
        => _attachments.Add(new OrderAttachment
        {
            Name = fileName,
            Attachment = file,
            TypeAttachment = contentType
        });

    public Order(string title, Money price, Guid customerId, Guid artistId, DateTime deadline)
    {
        Id = Guid.NewGuid();
        Title = title;
        Price = price;
        CustomerId = customerId;
        ArtistId = artistId;
        Deadline = deadline;
        Status = OrderStatus.New;
        CreatedDate = DateTime.UtcNow;
        IsActive = true;
    }

    public void UpdateStatus(OrderStatus newStatus)
    {
        if (Status == OrderStatus.Completed || Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Невозможно изменить статус выполненного или отмененного заказа");

        Status = newStatus;
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
    }
}