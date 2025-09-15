using System.Linq.Expressions;

using CommsManager.Models;
using CommsManager.Models.Enums;

namespace CommsManager.Interfaces;

public interface IRepository
{
    Task<List<Commission>> GetCommissionsAsync();
    Task<List<Commission>> GetCommissionsAsync(Expression<Func<Commission, bool>> predicate);
    Task<Commission?> GetCommissionByIdAsync(int id);
    Task AddCommissionAsync(Commission commission);
    Task UpdateCommissionAsync(Commission commission);
    Task DeleteCommissionAsync(int id);
    Task<List<Commission>> GetCommissionsByStatusAsync(CommissionStatus status);
    Task<List<Commission>> GetOverdueCommissionsAsync();
    Task<List<Commission>> GetCommissionsByCustomerAsync(int customerId);

    Task<List<Customer>> GetCustomersAsync();
    Task<Customer?> GetCustomerByIdAsync(int id);
    Task AddCustomerAsync(Customer customer);
    Task UpdateCustomerAsync(Customer customer);
    Task DeleteCustomerAsync(int id);
    Task<Customer?> GetCustomerByContactIdAsync(string contactId);

    Task<List<PortfolioItem>> GetPortfolioItemsAsync();
    Task<List<PortfolioItem>> GetPortfolioItemsAsync(Expression<Func<PortfolioItem, bool>> predicate);
    Task<PortfolioItem?> GetPortfolioItemByIdAsync(int id);
    Task AddPortfolioItemAsync(PortfolioItem portfolioItem);
    Task UpdatePortfolioItemAsync(PortfolioItem portfolioItem);
    Task DeletePortfolioItemAsync(int id);
    Task<List<PortfolioItem>> GetPortfolioItemsByTypeAsync(ArtType artType);
    Task<List<PortfolioItem>> GetFeaturedPortfolioItemsAsync();
    Task<List<PortfolioItem>> GetPortfolioItemsByTagAsync(string tagName);

    Task<List<PricePreset>> GetPricePresetsAsync();
    Task<PricePreset?> GetPricePresetByIdAsync(int id);
    Task AddPricePresetAsync(PricePreset pricePreset);
    Task UpdatePricePresetAsync(PricePreset pricePreset);
    Task DeletePricePresetAsync(int id);
    Task<List<PricePreset>> GetActivePricePresetsAsync();
    Task<PricePreset?> GetPricePresetByArtTypeAsync(ArtType artType);

    Task<List<Tag>> GetTagsAsync();
    Task<Tag?> GetTagByIdAsync(int id);
    Task<Tag?> GetTagByNameAsync(string name);
    Task AddTagAsync(Tag tag);
    Task UpdateTagAsync(Tag tag);
    Task DeleteTagAsync(int id);

    Task<List<AnalyticsRecord>> GetAnalyticsRecordsAsync();
    Task<AnalyticsRecord?> GetAnalyticsRecordByDateAsync(int year, int month);
    Task AddAnalyticsRecordAsync(AnalyticsRecord record);
    Task UpdateAnalyticsRecordAsync(AnalyticsRecord record);
    Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null);
    Task<int> GetCompletedCommissionsCountAsync(DateTime? startDate = null, DateTime? endDate = null);
    Task<Dictionary<ArtType, int>> GetCommissionStatisticsAsync(DateTime? startDate = null, DateTime? endDate = null);

    Task AddArtworkAsync(Artwork artwork);
    Task DeleteArtworkAsync(int id);
    Task<List<Artwork>> GetArtworksByCommissionAsync(int commissionId);

    Task<int> SaveChangesAsync();
    Task EnsureDatabaseCreatedAsync();
}
