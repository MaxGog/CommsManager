using CommsManager.Core.Entities;
using CommsManager.Core.Enums;

namespace CommsManager.Core.Interfaces.Repositories;

public interface IOrderRepository : IAsyncRepository<Order>
{
    Task<IReadOnlyList<Order>> GetOrdersByStatusAsync(
        OrderStatus status,
        Guid? artistId = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Order>> GetOrdersByCustomerAsync(
        Guid customerId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Order>> GetOverdueOrdersAsync(
        CancellationToken cancellationToken = default);

    Task<decimal> GetTotalRevenueAsync(
        Guid artistId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default);

    Task<int> GetActiveOrdersCountAsync(
        Guid artistId,
        CancellationToken cancellationToken = default);
}