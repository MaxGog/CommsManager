using Microsoft.EntityFrameworkCore;
using CommsManager.Core.Entities;
using CommsManager.Core.Interfaces;
using CommsManager.Infrastructure.Data;
using System.Linq.Expressions;

namespace CommsManager.Infrastructure.Repositories;

public class CustomerRepository(ApplicationDbContext context) : ICustomerRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .Include(c => c.Phones)
            .Include(c => c.Emails)
            .Include(c => c.SocialLinks)
            .Include(c => c.Orders)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .Include(c => c.Phones)
            .Include(c => c.Orders)
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Customer>> FindAsync(
        Expression<Func<Customer, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .Where(predicate)
            .Include(c => c.Phones)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Customer entity, CancellationToken cancellationToken = default)
    {
        await _context.Customers.AddAsync(entity, cancellationToken);
    }

    public Task UpdateAsync(Customer entity, CancellationToken cancellationToken = default)
    {
        _context.Customers.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Customer entity, CancellationToken cancellationToken = default)
    {
        _context.Customers.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Customers.AnyAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Customers.CountAsync(cancellationToken);
    }

    public async Task<int> CountAsync(
        Expression<Func<Customer, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.Customers.CountAsync(predicate, cancellationToken);
    }

    public async Task<IEnumerable<Customer>> SearchByNameAsync(
        string name, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .Where(c => c.Name.Contains(name))
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasOrdersAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return await _context.Orders.AnyAsync(o => o.CustomerId == customerId, cancellationToken);
    }

    public async Task<IEnumerable<Customer>> GetCustomersWithOrdersAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .Include(c => c.Orders)
            .Where(c => c.Orders.Any())
            .ToListAsync(cancellationToken);
    }
}