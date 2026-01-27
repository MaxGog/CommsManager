using Microsoft.EntityFrameworkCore.Storage;
using CommsManager.Core.Interfaces;
using CommsManager.Infrastructure.Repositories;

namespace CommsManager.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    private ICustomerRepository? _customers;
    private IOrderRepository? _orders;
    private IArtistProfileRepository? _artistProfiles;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public ICustomerRepository Customers =>
        _customers ??= new CustomerRepository(_context);

    public IOrderRepository Orders =>
        _orders ??= new OrderRepository(_context);

    public IArtistProfileRepository ArtistProfiles =>
        _artistProfiles ??= new ArtistProfileRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            throw new InvalidOperationException("Transaction not started");

        await _transaction.CommitAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            throw new InvalidOperationException("Transaction not started");

        await _transaction.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task<bool> SaveChangesWithTransactionAsync(CancellationToken cancellationToken = default)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var result = await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return result > 0;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction != null)
            await _transaction.DisposeAsync();
        await _context.DisposeAsync();
    }
}