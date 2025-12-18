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

public sealed class OrderNotFoundException : DomainException
{
    public Guid OrderId { get; }

    public OrderNotFoundException(Guid orderId) : base($"Заказ с идентификатором {OrderID} не найден")
    {
        OrderId = orderId;
    }
}

public sealed class InvalidOrderStateException : DomainException
{
    public InvalidOrderStateException(string message) : base(message)
    {
    }
}