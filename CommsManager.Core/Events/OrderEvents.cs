using CommsManager.Core.Entities;
using CommsManager.Core.Enums;

namespace CommsManager.Core.Events;

public sealed class OrderCreatedEvent : DomainEvent
{
    public Guid OrderId { get; }
    public string Title { get; }
    public decimal Amount { get; }
    public Guid CustomerId { get; }

    public OrderCreatedEvent(Order order)
    {
        OrderId = order.Id;
        Title = order.Title;
        Amount = order.Price.Amount;
        CustomerId = order.CustomerId;
    }
}

public sealed class OrderStatusChangedEvent : DomainEvent
{
    public Guid OrderId { get; }
    public OrderStatus NewStatus { get; }

    public OrderStatusChangedEvent(Guid orderId, OrderStatus newStatus)
    {
        OrderId = orderId;
        NewStatus = newStatus;
    }
}