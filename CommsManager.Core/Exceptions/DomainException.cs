using CommsManager.Core.Entities;

namespace CommsManager.Core.Exceptions;

public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message)
    {
    }

    protected DomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

public sealed class OrderNotFoundException(Guid orderId) : DomainException($"Заказ с идентификатором {orderId} не найден")
{
    public Guid OrderId { get; } = orderId;
}

public sealed class InvalidOrderStateException : DomainException
{
    public InvalidOrderStateException(string message) : base(message)
    {
    }
}