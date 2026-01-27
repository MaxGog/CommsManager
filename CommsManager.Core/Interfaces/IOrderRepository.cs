using CommsManager.Core.Entities;
using CommsManager.Core.Enums;

namespace CommsManager.Core.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Task<IEnumerable<Order>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetByArtistIdAsync(Guid artistId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetActiveOrdersAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetOverdueOrdersAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetOrdersWithAttachmentsAsync(CancellationToken cancellationToken = default);
    Task<decimal> GetTotalRevenueByArtistAsync(Guid artistId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default);
    Task<int> GetOrderCountByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default);
}