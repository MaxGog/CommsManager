using CommsManager.Core.Entities;
using CommsManager.Core.Enums;
using CommsManager.Core.Interfaces.Repositories;
using CommsManager.Core.Interfaces.Services;
using CommsManager.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace CommsManager.Core.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IArtistProfileRepository _artistProfileRepository;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        IArtistProfileRepository artistProfileRepository,
        ILogger<OrderService> logger)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _artistProfileRepository = artistProfileRepository;
        _logger = logger;
    }

    public async Task<Order> CreateOrderAsync(
        string title,
        string description,
        Money price,
        Guid customerId,
        Guid artistId,
        DateTime deadline,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Создание заказа для исполнителя {ArtistID}", artistId);

        if (deadline < DateTime.UtcNow)
            throw new ArgumentException("Крайний срок не может быть в прошлом");

        var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);
        if (customer == null)
            throw new KeyNotFoundException($"Клиент с идентификатором {CustomerID} не найден");

        var artist = await _artistProfileRepository.GetByIdAsync(artistId, cancellationToken);
        if (artist == null)
            throw new KeyNotFoundException($"Исполнитель с идентификатором {ArtistID} не найден");

        var order = new Order(title, description, price, customerId, deadline)
        {
            ArtistProfileId = artistId
        };

        var createdOrder = await _orderRepository.AddAsync(order, cancellationToken);

        _logger.LogInformation($"Заказ {OrderID} успешно создан", createdOrder.Id);

        return createdOrder;
    }

    public async Task<Order?> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await _orderRepository.FirstOrDefaultAsync(
            new OrderWithDetailsSpecification(orderId),
            cancellationToken);
    }

    public async Task<IReadOnlyList<Order>> GetOrdersByArtistAsync(
        Guid artistId,
        OrderStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        if (status.HasValue)
        {
            return await _orderRepository.ListAsync(
                new ActiveOrdersSpecification(artistId, status.Value),
                cancellationToken);
        }

        return await _orderRepository.ListAsync(
            new ActiveOrdersSpecification(artistId),
            cancellationToken);
    }

    public async Task UpdateOrderStatusAsync(
        Guid orderId,
        OrderStatus status,
        CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
        if (order == null)
            throw new KeyNotFoundException($"Заказ с идентификатором {OrderID} не найден");

        order.UpdateStatus(status);

        await _orderRepository.UpdateAsync(order, cancellationToken);

        _logger.LogInformation($"Статус заказа {OrderID} обновлен до {Status}", orderId, status);
    }

    public async Task<OrderStatistics> GetOrderStatisticsAsync(
        Guid artistId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        var orders = await _orderRepository.GetAsync(
            o => o.ArtistProfileId == artistId &&
                 o.CreatedDate >= startDate &&
                 o.CreatedDate <= endDate,
            cancellationToken);

        var totalOrders = orders.Count;
        var completedOrders = orders.Count(o => o.Status == OrderStatus.Completed);
        var inProgressOrders = orders.Count(o => o.Status == OrderStatus.InProgress);

        var totalRevenue = orders
            .Where(o => o.Status == OrderStatus.Completed)
            .Sum(o => o.Price.Amount);

        var averageOrderValue = totalOrders > 0 ? totalRevenue / totalOrders : 0;

        var ordersByStatus = orders
            .GroupBy(o => o.Status)
            .ToDictionary(g => g.Key, g => g.Count());

        return new OrderStatistics(
            totalOrders,
            completedOrders,
            inProgressOrders,
            new Money(totalRevenue),
            new Money(averageOrderValue),
            ordersByStatus);
    }
}