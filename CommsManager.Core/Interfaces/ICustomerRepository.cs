using CommsManager.Core.Entities;

namespace CommsManager.Core.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<IEnumerable<Customer>> SearchByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> HasOrdersAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Customer>> GetCustomersWithOrdersAsync(CancellationToken cancellationToken = default);
}