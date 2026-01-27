using CommsManager.Maui.Data.Models;
using CommsManager.Maui.Services;

namespace CommsManager.Maui.Data.Repositories;

public interface ICustomerRepository
{
    Task<IEnumerable<LocalCustomer>> GetAllAsync(bool includeInactive = false);
    Task<LocalCustomer?> GetByIdAsync(Guid id);
    Task<IEnumerable<LocalCustomer>> SearchAsync(string searchTerm);
    Task<LocalCustomer> CreateAsync(LocalCustomer customer);
    Task<bool> UpdateAsync(LocalCustomer customer);
    Task<bool> DeleteAsync(Guid id);
    Task<int> CountAsync(bool activeOnly = true);
    Task<bool> ExistsAsync(Guid id);
}

public class CustomerRepository : ICustomerRepository
{
    private readonly DatabaseService _databaseService;

    public CustomerRepository(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<IEnumerable<LocalCustomer>> GetAllAsync(bool includeInactive = false)
    {
        return await _databaseService.GetCustomersAsync(includeInactive);
    }

    public async Task<LocalCustomer?> GetByIdAsync(Guid id)
    {
        return await _databaseService.GetCustomerAsync(id);
    }

    public async Task<IEnumerable<LocalCustomer>> SearchAsync(string searchTerm)
    {
        return await _databaseService.SearchCustomersAsync(searchTerm);
    }

    public async Task<LocalCustomer> CreateAsync(LocalCustomer customer)
    {
        customer.Id = Guid.NewGuid();
        customer.CreatedDate = DateTime.UtcNow;

        await _databaseService.SaveCustomerAsync(customer);
        return customer;
    }

    public async Task<bool> UpdateAsync(LocalCustomer customer)
    {
        var existing = await GetByIdAsync(customer.Id);
        if (existing == null)
            return false;

        var result = await _databaseService.SaveCustomerAsync(customer);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var customer = await GetByIdAsync(id);
        if (customer == null)
            return false;

        var result = await _databaseService.DeleteCustomerAsync(customer);
        return result > 0;
    }

    public async Task<int> CountAsync(bool activeOnly = true)
    {
        return await _databaseService.GetCustomerCountAsync(activeOnly);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await GetByIdAsync(id) != null;
    }
}