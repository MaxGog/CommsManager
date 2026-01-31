using CommsManager.Maui.Data.Models;
using CommsManager.Maui.Data.Repositories;
using CommsManager.Maui.Interfaces;
using CommsManager.Maui.Models;
using CommsManager.Maui.Services;
using SQLite;

namespace CommsManager.Maui.Data;

public interface IUnitOfWork : IAsyncDisposable
{
    ICustomerRepository Customers { get; }
    IOrderRepository Orders { get; }
    IArtistRepository Artists { get; }
    ICommissionRepository Commissions { get; }

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task<bool> HasChangesAsync();

    Task<AppStatistics> GetStatisticsAsync();
    Task VacuumDatabaseAsync();
    Task ResetDatabaseAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly DatabaseService _databaseService;
    private SQLiteAsyncConnection? _connection;
    private bool _inTransaction = false;

    private ICustomerRepository? _customers;
    private IOrderRepository? _orders;
    private IArtistRepository? _artists;
    private ICommissionRepository? _commissions;

    private readonly IFileService _fileService;

    public UnitOfWork(DatabaseService databaseService, IFileService fileService)
    {
        _databaseService = databaseService;
        _fileService = fileService;
    }

    public ICustomerRepository Customers =>
        _customers ??= new CustomerRepository(_databaseService);

    public IOrderRepository Orders =>
        _orders ??= new OrderRepository(_databaseService);

    public IArtistRepository Artists =>
        _artists ??= new ArtistRepository(_databaseService, _fileService);

    public ICommissionRepository Commissions =>
        _commissions ??= new CommissionRepository(_databaseService, _fileService);

    public async Task<int> SaveChangesAsync()
    {
        return 1;
    }

    public async Task BeginTransactionAsync()
    {
        if (_connection == null)
        {
            _connection = await _databaseService.GetDatabaseAsync();
        }

        await _connection.ExecuteAsync("BEGIN TRANSACTION");
        _inTransaction = true;
    }

    public async Task CommitTransactionAsync()
    {
        if (!_inTransaction || _connection == null)
            throw new InvalidOperationException("Transaction not started");

        await _connection.ExecuteAsync("COMMIT");
        _inTransaction = false;
    }

    public async Task RollbackTransactionAsync()
    {
        if (!_inTransaction || _connection == null)
            throw new InvalidOperationException("Transaction not started");

        await _connection.ExecuteAsync("ROLLBACK");
        _inTransaction = false;
    }

    public Task<bool> HasChangesAsync()
    {
        return Task.FromResult(false);
    }

    public async Task<AppStatistics> GetStatisticsAsync()
    {
        return await _databaseService.GetStatisticsAsync();
    }

    public async Task VacuumDatabaseAsync()
    {
        await _databaseService.VacuumDatabaseAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        await _databaseService.ResetDatabaseAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (_inTransaction)
        {
            await RollbackTransactionAsync();
        }

        _connection = null;
    }
}