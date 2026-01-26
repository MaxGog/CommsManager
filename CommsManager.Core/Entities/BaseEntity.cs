using System.ComponentModel.DataAnnotations.Schema;
using CommsManager.Core.Events;

namespace CommsManager.Core.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; }

    private readonly List<IDomainEvent> _domainEvents = [];

    [NotMapped]
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}