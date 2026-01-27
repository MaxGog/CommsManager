using Microsoft.EntityFrameworkCore;
using CommsManager.Core.Entities;
using CommsManager.Core.Interfaces;
using CommsManager.Core.Enums;
using CommsManager.Infrastructure.Data;
using System.Linq.Expressions;

namespace CommsManager.Infrastructure.Repositories;

public class OrderRepository(ApplicationDbContext context) : IOrderRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Include(o => o.Attachments)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Include(o => o.Attachments)
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> FindAsync(
        Expression<Func<Order, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Where(predicate)
            .Include(o => o.Attachments)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Order entity, CancellationToken cancellationToken = default)
    {
        await _context.Orders.AddAsync(entity, cancellationToken);
    }

    public Task UpdateAsync(Order entity, CancellationToken cancellationToken = default)
    {
        _context.Orders.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Order entity, CancellationToken cancellationToken = default)
    {
        _context.Orders.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Orders.AnyAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Orders.CountAsync(cancellationToken);
    }

    public async Task<int> CountAsync(
        Expression<Func<Order, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.Orders.CountAsync(predicate, cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetByCustomerIdAsync(
        Guid customerId, CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Where(o => o.CustomerId == customerId)
            .Include(o => o.Attachments)
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetByArtistIdAsync(
        Guid artistId, CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Where(o => o.ArtistId == artistId)
            .Include(o => o.Attachments)
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetActiveOrdersAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Where(o => o.IsActive)
            .Include(o => o.Attachments)
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetOverdueOrdersAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Where(o => o.IsOverdue)
            .Include(o => o.Attachments)
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(
        OrderStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Where(o => o.Status == status)
            .Include(o => o.Attachments)
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetOrdersWithAttachmentsAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Where(o => o.Attachments.Any())
            .Include(o => o.Attachments)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetTotalRevenueByArtistAsync(
        Guid artistId, DateTime? startDate = null, DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Orders
            .Where(o => o.ArtistId == artistId && o.Status == OrderStatus.Completed);

        if (startDate.HasValue)
        {
            query = query.Where(o => o.CreatedDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(o => o.CreatedDate <= endDate.Value);
        }

        return await query.SumAsync(o => o.Price.Amount, cancellationToken);
    }

    public async Task<int> GetOrderCountByStatusAsync(
        OrderStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.Orders.CountAsync(o => o.Status == status, cancellationToken);
    }
}