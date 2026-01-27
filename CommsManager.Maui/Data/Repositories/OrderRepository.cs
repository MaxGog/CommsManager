using CommsManager.Maui.Data.Models;
using CommsManager.Maui.Models;
using CommsManager.Maui.Services;

namespace CommsManager.Maui.Data.Repositories;

public interface IOrderRepository
{
    Task<IEnumerable<LocalOrder>> GetAllAsync(OrderFilter? filter = null);
    Task<LocalOrder?> GetByIdAsync(Guid id);
    Task<IEnumerable<LocalOrder>> GetByCustomerIdAsync(Guid customerId);
    Task<IEnumerable<LocalOrder>> GetByArtistIdAsync(Guid artistId);
    Task<LocalOrder> CreateAsync(LocalOrder order);
    Task<bool> UpdateAsync(LocalOrder order);
    Task<bool> DeleteAsync(Guid id);

    Task<IEnumerable<LocalOrder>> GetByStatusAsync(string status);
    Task<IEnumerable<LocalOrder>> GetActiveOrdersAsync();
    Task<IEnumerable<LocalOrder>> GetOverdueOrdersAsync();
    Task<IEnumerable<LocalOrder>> GetOrdersDueSoonAsync(int days = 3);

    Task<int> CountAsync(OrderFilter? filter = null);
    Task<decimal> GetTotalRevenueAsync(DateTime? fromDate = null, DateTime? toDate = null);
    Task<Dictionary<string, int>> GetStatusStatisticsAsync();

    Task<bool> ExistsAsync(Guid id);
    Task<bool> ChangeStatusAsync(Guid orderId, string newStatus);
    Task<bool> UpdateDeadlineAsync(Guid orderId, DateTime newDeadline);
    Task<bool> UpdatePriceAsync(Guid orderId, decimal newPrice);
}

public class OrderRepository : IOrderRepository
{
    private readonly DatabaseService _databaseService;

    public OrderRepository(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<IEnumerable<LocalOrder>> GetAllAsync(OrderFilter? filter = null)
    {
        return await _databaseService.GetOrdersAsync(filter);
    }

    public async Task<LocalOrder?> GetByIdAsync(Guid id)
    {
        return await _databaseService.GetOrderAsync(id);
    }

    public async Task<IEnumerable<LocalOrder>> GetByCustomerIdAsync(Guid customerId)
    {
        var filter = new OrderFilter { CustomerId = customerId };
        return await _databaseService.GetOrdersAsync(filter);
    }

    public async Task<IEnumerable<LocalOrder>> GetByArtistIdAsync(Guid artistId)
    {
        var filter = new OrderFilter { ArtistId = artistId };
        return await _databaseService.GetOrdersAsync(filter);
    }

    public async Task<LocalOrder> CreateAsync(LocalOrder order)
    {
        if (order.Id == Guid.Empty)
            order.Id = Guid.NewGuid();

        if (order.CreatedDate == default)
            order.CreatedDate = DateTime.UtcNow;

        await _databaseService.SaveOrderAsync(order);
        return order;
    }

    public async Task<bool> UpdateAsync(LocalOrder order)
    {
        var existing = await GetByIdAsync(order.Id);
        if (existing == null)
            return false;

        var result = await _databaseService.SaveOrderAsync(order);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var order = await GetByIdAsync(id);
        if (order == null)
            return false;

        var result = await _databaseService.DeleteOrderAsync(order);
        return result > 0;
    }

    public async Task<IEnumerable<LocalOrder>> GetByStatusAsync(string status)
    {
        return await _databaseService.GetOrdersByStatusAsync(status);
    }

    public async Task<IEnumerable<LocalOrder>> GetActiveOrdersAsync()
    {
        var filter = new OrderFilter { IsActive = true };
        return await _databaseService.GetOrdersAsync(filter);
    }

    public async Task<IEnumerable<LocalOrder>> GetOverdueOrdersAsync()
    {
        return await _databaseService.GetOverdueOrdersAsync();
    }

    public async Task<IEnumerable<LocalOrder>> GetOrdersDueSoonAsync(int days = 3)
    {
        return await _databaseService.GetOrdersDueSoonAsync(days);
    }

    public async Task<int> CountAsync(OrderFilter? filter = null)
    {
        var orders = await _databaseService.GetOrdersAsync(filter);
        return orders.Count;
    }

    public async Task<decimal> GetTotalRevenueAsync(DateTime? fromDate = null, DateTime? toDate = null)
    {
        return await _databaseService.GetTotalRevenueAsync(fromDate, toDate);
    }

    public async Task<Dictionary<string, int>> GetStatusStatisticsAsync()
    {
        var orders = await _databaseService.GetOrdersAsync();

        var statistics = new Dictionary<string, int>
        {
            ["New"] = 0,
            ["InProgress"] = 0,
            ["Pending"] = 0,
            ["Completed"] = 0,
            ["Cancelled"] = 0
        };

        foreach (var order in orders)
        {
            if (statistics.ContainsKey(order.Status))
            {
                statistics[order.Status]++;
            }
            else
            {
                statistics[order.Status] = 1;
            }
        }

        return statistics;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await GetByIdAsync(id) != null;
    }

    public async Task<bool> ChangeStatusAsync(Guid orderId, string newStatus)
    {
        var order = await GetByIdAsync(orderId);
        if (order == null)
            return false;

        order.Status = newStatus;

        if (newStatus == "Completed" || newStatus == "Cancelled")
        {
            order.IsActive = false;
        }

        return await UpdateAsync(order);
    }

    public async Task<bool> UpdateDeadlineAsync(Guid orderId, DateTime newDeadline)
    {
        var order = await GetByIdAsync(orderId);
        if (order == null)
            return false;

        order.Deadline = newDeadline;
        return await UpdateAsync(order);
    }

    public async Task<bool> UpdatePriceAsync(Guid orderId, decimal newPrice)
    {
        var order = await GetByIdAsync(orderId);
        if (order == null)
            return false;

        order.PriceAmount = newPrice;
        return await UpdateAsync(order);
    }
}