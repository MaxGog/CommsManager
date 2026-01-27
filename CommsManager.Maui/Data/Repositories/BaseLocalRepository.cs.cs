using CommsManager.Maui.Data.Models;
using CommsManager.Maui.Services;

namespace CommsManager.Maui.Data.Repositories;

public interface ILocalRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task<T> CreateAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(Guid id);
    Task<int> CountAsync();
    Task<bool> ExistsAsync(Guid id);
}

public abstract class BaseLocalRepository<T> : ILocalRepository<T> where T : class
{
    protected readonly DatabaseService _databaseService;

    protected BaseLocalRepository(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public abstract Task<IEnumerable<T>> GetAllAsync();
    public abstract Task<T?> GetByIdAsync(Guid id);
    public abstract Task<T> CreateAsync(T entity);
    public abstract Task<bool> UpdateAsync(T entity);
    public abstract Task<bool> DeleteAsync(Guid id);

    public virtual async Task<int> CountAsync()
    {
        var entities = await GetAllAsync();
        return entities.Count();
    }

    public virtual async Task<bool> ExistsAsync(Guid id)
    {
        return await GetByIdAsync(id) != null;
    }
}