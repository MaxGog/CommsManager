using MediatR;

namespace CommsManager.Core.Events;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}

public abstract class DomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}