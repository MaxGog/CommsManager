using CommsManager.Maui.Data.Models;
using CommsManager.Maui.Enums;
using CommsManager.Maui.Interfaces;
using CommsManager.Maui.Services;
using System.Text.Json;

namespace CommsManager.Maui.Data.Repositories;

public interface ICommissionRepository
{
    Task<IEnumerable<LocalCommission>> GetAllAsync();
    Task<LocalCommission?> GetByIdAsync(Guid id);
    Task<IEnumerable<LocalCommission>> GetByArtistIdAsync(Guid artistId);
    Task<IEnumerable<LocalCommission>> SearchAsync(string searchTerm);
    Task<LocalCommission> CreateAsync(LocalCommission commission);
    Task<bool> UpdateAsync(LocalCommission commission);
    Task<bool> DeleteAsync(Guid id);

    Task<IEnumerable<LocalCommission>> GetByTypeAsync(string type);
    Task<IEnumerable<LocalCommission>> GetPopularCommissionsAsync(int count = 10);

    Task<bool> AddViewAttachmentAsync(Guid commissionId, byte[] imageData);
    Task<bool> RemoveViewAttachmentAsync(Guid commissionId, int attachmentIndex);
    Task<List<byte[]>?> GetViewAttachmentsAsync(Guid commissionId);
    Task<bool> ClearViewAttachmentsAsync(Guid commissionId);

    Task<int> CountAsync();
    Task<int> CountByArtistAsync(Guid artistId);
    Task<Dictionary<string, int>> GetTypeStatisticsAsync(Guid? artistId = null);
    Task<bool> ExistsAsync(Guid id);
}

public class CommissionRepository : ICommissionRepository
{
    private readonly DatabaseService _databaseService;
    private readonly IFileService _fileService;

    public CommissionRepository(DatabaseService databaseService, IFileService fileService)
    {
        _databaseService = databaseService;
        _fileService = fileService;
    }

    public async Task<IEnumerable<LocalCommission>> GetAllAsync()
    {
        var allArtists = await _databaseService.GetArtistProfilesAsync();
        var allCommissions = new List<LocalCommission>();

        foreach (var artist in allArtists)
        {
            var commissions = await _databaseService.GetCommissionsByArtistAsync(artist.Id);
            allCommissions.AddRange(commissions);
        }

        return allCommissions;
    }

    public async Task<LocalCommission?> GetByIdAsync(Guid id)
    {
        return await _databaseService.GetCommissionAsync(id);
    }

    public async Task<IEnumerable<LocalCommission>> GetByArtistIdAsync(Guid artistId)
    {
        return await _databaseService.GetCommissionsByArtistAsync(artistId);
    }

    public async Task<IEnumerable<LocalCommission>> SearchAsync(string searchTerm)
    {
        var allCommissions = await GetAllAsync();

        return allCommissions
            .Where(c => c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                       (c.Description != null && c.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                       (c.TypeCommission != null && c.TypeCommission.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
            .OrderBy(c => c.Name)
            .ToList();
    }

    public async Task<LocalCommission> CreateAsync(LocalCommission commission)
    {
        if (commission.Id == Guid.Empty)
            commission.Id = Guid.NewGuid();

        if (commission.CreatedDate == default)
            commission.CreatedDate = DateTime.UtcNow;

        await _databaseService.SaveCommissionAsync(commission);
        return commission;
    }

    public async Task<bool> UpdateAsync(LocalCommission commission)
    {
        var existing = await GetByIdAsync(commission.Id);
        if (existing == null)
            return false;

        commission.UpdatedDate = DateTime.UtcNow;
        var result = await _databaseService.SaveCommissionAsync(commission);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var commission = await GetByIdAsync(id);
        if (commission == null)
            return false;

        await ClearViewAttachmentsAsync(id);

        var result = await _databaseService.DeleteCommissionAsync(commission);
        return result > 0;
    }

    public async Task<IEnumerable<LocalCommission>> GetByTypeAsync(string type)
    {
        var allCommissions = await GetAllAsync();
        return allCommissions
            .Where(c => c.TypeCommission == type)
            .OrderBy(c => c.Name)
            .ToList();
    }

    public async Task<IEnumerable<LocalCommission>> GetPopularCommissionsAsync(int count = 10)
    {
        var allCommissions = await GetAllAsync();

        return [.. allCommissions
            .OrderBy(c => c.Name)
            .Take(count)];
    }

    public async Task<bool> AddViewAttachmentAsync(Guid commissionId, byte[] imageData)
    {
        var commission = await GetByIdAsync(commissionId);
        if (commission == null)
            return false;

        try
        {
            var attachments = commission.ViewAttachment ?? [];

            attachments.Add(imageData);

            var filePath = await _fileService.SaveImageForEntityAsync(
                imageData, commissionId, EntityImageType.CommissionPreview);

            commission.ViewAttachment = attachments;
            commission.ViewAttachmentJson = JsonSerializer.Serialize(attachments);

            return await UpdateAsync(commission);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding view attachment: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RemoveViewAttachmentAsync(Guid commissionId, int attachmentIndex)
    {
        var commission = await GetByIdAsync(commissionId);
        if (commission == null)
            return false;

        var attachments = commission.ViewAttachment;
        if (attachments == null || attachmentIndex < 0 || attachmentIndex >= attachments.Count)
            return false;

        attachments.RemoveAt(attachmentIndex);
        commission.ViewAttachment = attachments;
        commission.ViewAttachmentJson = JsonSerializer.Serialize(attachments);

        return await UpdateAsync(commission);
    }

    public async Task<List<byte[]>?> GetViewAttachmentsAsync(Guid commissionId)
    {
        var commission = await GetByIdAsync(commissionId);
        return commission?.ViewAttachment;
    }

    public async Task<bool> ClearViewAttachmentsAsync(Guid commissionId)
    {
        var commission = await GetByIdAsync(commissionId);
        if (commission == null)
            return false;

        commission.ViewAttachment = [];
        commission.ViewAttachmentJson = JsonSerializer.Serialize(new List<byte[]>());

        return await UpdateAsync(commission);
    }

    public async Task<int> CountAsync()
    {
        var allCommissions = await GetAllAsync();
        return allCommissions.Count();
    }

    public async Task<int> CountByArtistAsync(Guid artistId)
    {
        var commissions = await GetByArtistIdAsync(artistId);
        return commissions.Count();
    }

    public async Task<Dictionary<string, int>> GetTypeStatisticsAsync(Guid? artistId = null)
    {
        IEnumerable<LocalCommission> commissions;

        if (artistId.HasValue)
        {
            commissions = await GetByArtistIdAsync(artistId.Value);
        }
        else
        {
            commissions = await GetAllAsync();
        }

        var statistics = new Dictionary<string, int>();

        foreach (var commission in commissions)
        {
            var type = commission.TypeCommission ?? "Other";

            if (statistics.ContainsKey(type))
            {
                statistics[type]++;
            }
            else
            {
                statistics[type] = 1;
            }
        }

        return statistics;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await GetByIdAsync(id) != null;
    }
}