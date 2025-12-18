using CommsManager.Core.Entities;
using CommsManager.Core.Enums;
using CommsManager.Core.ValueObjects;

namespace CommsManager.Core.Interfaces.Services;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(
        string title,
        string description,
        Money price,
        Guid customerId,
        Guid artistId,
        DateTime deadline,
        CancellationToken cancellationToken = default);

    Task<Order?> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Order>> GetOrdersByArtistAsync(
        Guid artistId,
        OrderStatus? status = null,
        CancellationToken cancellationToken = default);

    Task UpdateOrderStatusAsync(
        Guid orderId,
        OrderStatus status,
        CancellationToken cancellationToken = default);

    Task AddOrderAttachmentAsync(
        Guid orderId,
        string fileName,
        string fileUrl,
        string contentType,
        CancellationToken cancellationToken = default);

    Task AddOrderNoteAsync(
        Guid orderId,
        string content,
        Guid createdBy,
        CancellationToken cancellationToken = default);

    Task<OrderStatistics> GetOrderStatisticsAsync(
        Guid artistId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default);
}

public record OrderStatistics(
    int TotalOrders,
    int CompletedOrders,
    int InProgressOrders,
    Money TotalRevenue,
    Money AverageOrderValue,
    Dictionary<OrderStatus, int> OrdersByStatus);