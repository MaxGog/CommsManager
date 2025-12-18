public interface IOrderService
{
    Task<Order> CreateOrderAsync(Order order);
    Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);
    Task UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
    Task<decimal> CalculateTotalRevenueAsync(DateTime start, DateTime end);
}

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;

    public async Task<Order> CreateOrderAsync(Order order)
    {
        order.Id = Guid.NewGuid();
        order.CreatedDate = DateTime.UtcNow;
        order.Status = OrderStatus.New;

        return await _repository.AddAsync(order);
    }
}