using CommsManager.Maui.Data.Models;
using CommsManager.Maui.Services;
using System.Text.Json;

namespace CommsManager.Maui.Data.Repositories;

public interface IArtistRepository
{
    Task<IEnumerable<LocalArtistProfile>> GetAllAsync();
    Task<LocalArtistProfile?> GetByIdAsync(Guid id);
    Task<IEnumerable<LocalArtistProfile>> SearchAsync(string searchTerm);
    Task<LocalArtistProfile> CreateAsync(LocalArtistProfile artist);
    Task<bool> UpdateAsync(LocalArtistProfile artist);
    Task<bool> DeleteAsync(Guid id);

    Task<IEnumerable<LocalArtistProfile>> GetPopularArtistsAsync(int count = 10);
    Task<IEnumerable<LocalArtistProfile>> GetArtistsWithCommissionsAsync();

    Task<bool> SetArtistPictureAsync(Guid artistId, byte[] imageData);
    Task<bool> SetArtistBannerAsync(Guid artistId, byte[] imageData);
    Task<byte[]?> GetArtistPictureAsync(Guid artistId);
    Task<byte[]?> GetArtistBannerAsync(Guid artistId);

    Task<int> CountAsync();
    Task<bool> ExistsAsync(Guid id);
    Task<int> GetCommissionCountAsync(Guid artistId);
    Task<int> GetOrderCountAsync(Guid artistId);
}

public class ArtistRepository : IArtistRepository
{
    private readonly DatabaseService _databaseService;
    private readonly IFileService _fileService;

    public ArtistRepository(DatabaseService databaseService, IFileService fileService)
    {
        _databaseService = databaseService;
        _fileService = fileService;
    }

    public async Task<IEnumerable<LocalArtistProfile>> GetAllAsync()
    {
        return await _databaseService.GetArtistProfilesAsync();
    }

    public async Task<LocalArtistProfile?> GetByIdAsync(Guid id)
    {
        return await _databaseService.GetArtistProfileAsync(id);
    }

    public async Task<IEnumerable<LocalArtistProfile>> SearchAsync(string searchTerm)
    {
        return await _databaseService.SearchArtistsAsync(searchTerm);
    }

    public async Task<LocalArtistProfile> CreateAsync(LocalArtistProfile artist)
    {
        if (artist.Id == Guid.Empty)
            artist.Id = Guid.NewGuid();

        if (artist.CreatedDate == default)
            artist.CreatedDate = DateTime.UtcNow;

        await _databaseService.SaveArtistProfileAsync(artist);
        return artist;
    }

    public async Task<bool> UpdateAsync(LocalArtistProfile artist)
    {
        var existing = await GetByIdAsync(artist.Id);
        if (existing == null)
            return false;

        var result = await _databaseService.SaveArtistProfileAsync(artist);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var artist = await GetByIdAsync(id);
        if (artist == null)
            return false;

        var commissions = await _databaseService.GetCommissionsByArtistAsync(id);
        foreach (var commission in commissions)
        {
            await _databaseService.DeleteCommissionAsync(commission);
        }

        if (!string.IsNullOrEmpty(artist.ArtistPicturePath))
        {
            await _fileService.DeleteImageAsync(artist.ArtistPicturePath);
        }

        if (!string.IsNullOrEmpty(artist.ArtistBannerPath))
        {
            await _fileService.DeleteImageAsync(artist.ArtistBannerPath);
        }

        var result = await _databaseService.DeleteArtistProfileAsync(artist);
        return result > 0;
    }

    public async Task<IEnumerable<LocalArtistProfile>> GetPopularArtistsAsync(int count = 10)
    {
        var allArtists = await GetAllAsync();
        var artistsWithStats = new List<(LocalArtistProfile Artist, int CommissionCount)>();

        foreach (var artist in allArtists)
        {
            var commissionCount = await _databaseService.QueryAsync<int>(
                "SELECT COUNT(*) FROM Commissions WHERE ArtistProfileId = ?", artist.Id);

            artistsWithStats.Add((artist, commissionCount.FirstOrDefault()));
        }

        return artistsWithStats
            .OrderByDescending(x => x.CommissionCount)
            .Take(count)
            .Select(x => x.Artist)
            .ToList();
    }

    public async Task<IEnumerable<LocalArtistProfile>> GetArtistsWithCommissionsAsync()
    {
        var allArtists = await GetAllAsync();
        var artistsWithCommissions = new List<LocalArtistProfile>();

        foreach (var artist in allArtists)
        {
            var commissions = await _databaseService.GetCommissionsByArtistAsync(artist.Id);
            if (commissions.Any())
            {
                artistsWithCommissions.Add(artist);
            }
        }

        return artistsWithCommissions;
    }

    public async Task<bool> SetArtistPictureAsync(Guid artistId, byte[] imageData)
    {
        var artist = await GetByIdAsync(artistId);
        if (artist == null)
            return false;

        try
        {
            var filePath = await _fileService.SaveImageForEntityAsync(
                imageData, artistId, EntityImageType.ArtistPicture);

            artist.ArtistPicturePath = filePath;

            artist.ArtistPictureBase64 = Convert.ToBase64String(imageData);

            return await UpdateAsync(artist);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting artist picture: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> SetArtistBannerAsync(Guid artistId, byte[] imageData)
    {
        var artist = await GetByIdAsync(artistId);
        if (artist == null)
            return false;

        try
        {
            var filePath = await _fileService.SaveImageForEntityAsync(
                imageData, artistId, EntityImageType.ArtistBanner);

            artist.ArtistBannerPath = filePath;
            artist.ArtistBannerBase64 = Convert.ToBase64String(imageData);

            return await UpdateAsync(artist);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting artist banner: {ex.Message}");
            return false;
        }
    }

    public async Task<byte[]?> GetArtistPictureAsync(Guid artistId)
    {
        var artist = await GetByIdAsync(artistId);
        if (artist == null)
            return null;

        if (!string.IsNullOrEmpty(artist.ArtistPictureBase64))
        {
            try
            {
                return Convert.FromBase64String(artist.ArtistPictureBase64);
            }
            catch
            {
            }
        }

        if (!string.IsNullOrEmpty(artist.ArtistPicturePath))
        {
            return await _fileService.LoadImageBytesAsync(artist.ArtistPicturePath);
        }

        return null;
    }

    public async Task<byte[]?> GetArtistBannerAsync(Guid artistId)
    {
        var artist = await GetByIdAsync(artistId);
        if (artist == null)
            return null;

        if (!string.IsNullOrEmpty(artist.ArtistBannerBase64))
        {
            try
            {
                return Convert.FromBase64String(artist.ArtistBannerBase64);
            }
            catch
            {
            }
        }

        if (!string.IsNullOrEmpty(artist.ArtistBannerPath))
        {
            return await _fileService.LoadImageBytesAsync(artist.ArtistBannerPath);
        }

        return null;
    }

    public async Task<int> CountAsync()
    {
        var artists = await GetAllAsync();
        return artists.Count();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await GetByIdAsync(id) != null;
    }

    public async Task<int> GetCommissionCountAsync(Guid artistId)
    {
        var commissions = await _databaseService.GetCommissionsByArtistAsync(artistId);
        return commissions.Count;
    }

    public async Task<int> GetOrderCountAsync(Guid artistId)
    {
        var filter = new OrderFilter { ArtistId = artistId };
        var orders = await _databaseService.GetOrdersAsync(filter);
        return orders.Count;
    }
}