using System.Linq.Expressions;

using CommsManager.Interfaces;
using CommsManager.Models;
using CommsManager.Models.Enums;

using Microsoft.EntityFrameworkCore;

namespace CommsManager.Services.Database;

public class Repository : IRepository, IDisposable
{
    private readonly DatabaseContext _context;
    private bool _disposed = false;

    public Repository(DatabaseContext context)
    {
        _context = context;
        _context.Database.EnsureCreated();
    }

    #region Commission Methods

    public async Task<List<Commission>> GetCommissionsAsync()
    {
        return await _context.Commissions
            .Include(c => c.Customer)
            .Include(c => c.Tags)
                .ThenInclude(ct => ct.Tag)
            .Include(c => c.Artworks)
            .OrderByDescending(c => c.CreatedDate)
            .ToListAsync();
    }

    public async Task<List<Commission>> GetCommissionsAsync(Expression<Func<Commission, bool>> predicate)
    {
        return await _context.Commissions
            .Include(c => c.Customer)
            .Include(c => c.Tags)
                .ThenInclude(ct => ct.Tag)
            .Include(c => c.Artworks)
            .Where(predicate)
            .OrderByDescending(c => c.CreatedDate)
            .ToListAsync();
    }

    public async Task<Commission?> GetCommissionByIdAsync(int id)
    {
        return await _context.Commissions
            .Include(c => c.Customer)
            .Include(c => c.Tags)
                .ThenInclude(ct => ct.Tag)
            .Include(c => c.Artworks)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddCommissionAsync(Commission commission)
    {
        await _context.Commissions.AddAsync(commission);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCommissionAsync(Commission commission)
    {
        _context.Commissions.Update(commission);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCommissionAsync(int id)
    {
        var commission = await GetCommissionByIdAsync(id);
        if (commission != null)
        {
            _context.Commissions.Remove(commission);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Commission>> GetCommissionsByStatusAsync(CommissionStatus status)
    {
        return await GetCommissionsAsync(c => c.Status == status);
    }

    public async Task<List<Commission>> GetOverdueCommissionsAsync()
    {
        return await GetCommissionsAsync(c => c.Deadline.HasValue && 
                                            c.Deadline.Value < DateTime.UtcNow && 
                                            c.Status != CommissionStatus.Completed);
    }

    public async Task<List<Commission>> GetCommissionsByCustomerAsync(int customerId)
    {
        return await GetCommissionsAsync(c => c.CustomerId == customerId);
    }

    public async Task<List<Commission>> GetCommissionsByArtTypeAsync(ArtType artType)
    {
        return await GetCommissionsAsync(c => c.ArtType == artType);
    }

    public async Task<List<Commission>> GetCommissionsByArtTypeAndStatusAsync(ArtType artType, CommissionStatus status)
    {
        return await GetCommissionsAsync(c => c.ArtType == artType && c.Status == status);
    }

    #endregion

    #region Customer Methods

    public async Task<List<Customer>> GetCustomersAsync()
    {
        return await _context.Customers
            .Include(c => c.Commissions)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Customer?> GetCustomerByIdAsync(int id)
    {
        return await _context.Customers
            .Include(c => c.Commissions)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddCustomerAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCustomerAsync(int id)
    {
        var customer = await GetCustomerByIdAsync(id);
        if (customer != null)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Customer?> GetCustomerByContactIdAsync(string contactId)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.ContactId == contactId);
    }

    #endregion

    #region Portfolio Methods

    public async Task<List<PortfolioItem>> GetPortfolioItemsAsync()
    {
        return await _context.PortfolioItems
            .Include(p => p.Tags)
                .ThenInclude(pt => pt.Tag)
            .Include(p => p.Commission)
            .OrderByDescending(p => p.CreatedDate)
            .ToListAsync();
    }

    public async Task<List<PortfolioItem>> GetPortfolioItemsAsync(Expression<Func<PortfolioItem, bool>> predicate)
    {
        return await _context.PortfolioItems
            .Include(p => p.Tags)
                .ThenInclude(pt => pt.Tag)
            .Include(p => p.Commission)
            .Where(predicate)
            .OrderByDescending(p => p.CreatedDate)
            .ToListAsync();
    }

    public async Task<PortfolioItem?> GetPortfolioItemByIdAsync(int id)
    {
        return await _context.PortfolioItems
            .Include(p => p.Tags)
                .ThenInclude(pt => pt.Tag)
            .Include(p => p.Commission)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddPortfolioItemAsync(PortfolioItem portfolioItem)
    {
        await _context.PortfolioItems.AddAsync(portfolioItem);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePortfolioItemAsync(PortfolioItem portfolioItem)
    {
        _context.PortfolioItems.Update(portfolioItem);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePortfolioItemAsync(int id)
    {
        var portfolioItem = await GetPortfolioItemByIdAsync(id);
        if (portfolioItem != null)
        {
            _context.PortfolioItems.Remove(portfolioItem);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<PortfolioItem>> GetPortfolioItemsByTypeAsync(ArtType artType)
    {
        return await GetPortfolioItemsAsync(p => p.ArtType == artType);
    }

    public async Task<List<PortfolioItem>> GetFeaturedPortfolioItemsAsync()
    {
        return await GetPortfolioItemsAsync(p => p.IsFeatured);
    }

    public async Task<List<PortfolioItem>> GetPortfolioItemsByTagAsync(string tagName)
    {
        return await _context.PortfolioItems
            .Include(p => p.Tags)
                .ThenInclude(pt => pt.Tag)
            .Where(p => p.Tags.Any(pt => pt.Tag.Name == tagName))
            .OrderByDescending(p => p.CreatedDate)
            .ToListAsync();
    }

    #endregion

    #region PricePreset Methods

    public async Task<List<PricePreset>> GetPricePresetsAsync()
    {
        return await _context.PricePresets
            .OrderBy(p => p.DisplayOrder)
            .ToListAsync();
    }

    public async Task<PricePreset?> GetPricePresetByIdAsync(int id)
    {
        return await _context.PricePresets.FindAsync(id);
    }

    public async Task AddPricePresetAsync(PricePreset pricePreset)
    {
        await _context.PricePresets.AddAsync(pricePreset);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePricePresetAsync(PricePreset pricePreset)
    {
        _context.PricePresets.Update(pricePreset);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePricePresetAsync(int id)
    {
        var pricePreset = await GetPricePresetByIdAsync(id);
        if (pricePreset != null)
        {
            _context.PricePresets.Remove(pricePreset);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<PricePreset>> GetActivePricePresetsAsync()
    {
        return await _context.PricePresets
            .Where(p => p.IsActive)
            .OrderBy(p => p.DisplayOrder)
            .ToListAsync();
    }

    public async Task<PricePreset?> GetPricePresetByArtTypeAsync(ArtType artType)
    {
        return await _context.PricePresets
            .FirstOrDefaultAsync(p => p.ArtType == artType && p.IsActive);
    }

    #endregion

    #region Tag Methods

    public async Task<List<Tag>> GetTagsAsync()
    {
        return await _context.Tags
            .OrderBy(t => t.Name)
            .ToListAsync();
    }

    public async Task<Tag?> GetTagByIdAsync(int id)
    {
        return await _context.Tags.FindAsync(id);
    }

    public async Task<Tag?> GetTagByNameAsync(string name)
    {
        return await _context.Tags
            .FirstOrDefaultAsync(t => t.Name == name);
    }

    public async Task AddTagAsync(Tag tag)
    {
        await _context.Tags.AddAsync(tag);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTagAsync(Tag tag)
    {
        _context.Tags.Update(tag);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTagAsync(int id)
    {
        var tag = await GetTagByIdAsync(id);
        if (tag != null)
        {
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
        }
    }

    #endregion

    #region Analytics Methods

    public async Task<List<AnalyticsRecord>> GetAnalyticsRecordsAsync()
    {
        return await _context.AnalyticsRecords
            .OrderByDescending(a => a.Year)
            .ThenByDescending(a => a.Month)
            .ToListAsync();
    }

    public async Task<AnalyticsRecord?> GetAnalyticsRecordByDateAsync(int year, int month)
    {
        return await _context.AnalyticsRecords
            .FirstOrDefaultAsync(a => a.Year == year && a.Month == month);
    }

    public async Task AddAnalyticsRecordAsync(AnalyticsRecord record)
    {
        await _context.AnalyticsRecords.AddAsync(record);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAnalyticsRecordAsync(AnalyticsRecord record)
    {
        _context.AnalyticsRecords.Update(record);
        await _context.SaveChangesAsync();
    }

    public async Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.Commissions
            .Where(c => c.Status == CommissionStatus.Completed && c.IsPaid);

        if (startDate.HasValue)
            query = query.Where(c => c.CreatedDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(c => c.CreatedDate <= endDate.Value);

        return await query.SumAsync(c => c.Price);
    }

    public async Task<int> GetCompletedCommissionsCountAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.Commissions
            .Where(c => c.Status == CommissionStatus.Completed);

        if (startDate.HasValue)
            query = query.Where(c => c.CreatedDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(c => c.CreatedDate <= endDate.Value);

        return await query.CountAsync();
    }

    public async Task<Dictionary<ArtType, int>> GetCommissionStatisticsAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.Commissions
            .Where(c => c.Status == CommissionStatus.Completed);

        if (startDate.HasValue)
            query = query.Where(c => c.CreatedDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(c => c.CreatedDate <= endDate.Value);

        var statistics = await query
            .GroupBy(c => c.ArtType)
            .Select(g => new { ArtType = g.Key, Count = g.Count() })
            .ToListAsync();

        return statistics.ToDictionary(s => s.ArtType, s => s.Count);
    }
    #endregion

    #region Artwork Methods

    public async Task AddArtworkAsync(Artwork artwork)
    {
        await _context.Artworks.AddAsync(artwork);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteArtworkAsync(int id)
    {
        var artwork = await _context.Artworks.FindAsync(id);
        if (artwork != null)
        {
            _context.Artworks.Remove(artwork);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Artwork>> GetArtworksByCommissionAsync(int commissionId)
    {
        return await _context.Artworks
            .Where(a => a.CommissionId == commissionId)
            .OrderByDescending(a => a.UploadDate)
            .ToListAsync();
    }

    #endregion

    #region General Methods

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task EnsureDatabaseCreatedAsync()
    {
        await _context.Database.EnsureCreatedAsync();
    }

    #endregion

    #region IDisposable Implementation

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }

    #endregion
}