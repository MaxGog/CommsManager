using CommsManager.Core.Entities;

namespace CommsManager.Core.Interfaces;

public interface IArtistProfileRepository : IRepository<ArtistProfile>
{
    Task<ArtistProfile?> GetProfileWithCommissionsAsync(Guid artistId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ArtistProfile>> SearchByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> HasActiveCommissionsAsync(Guid artistId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ArtistProfile>> GetPopularArtistsAsync(int count, CancellationToken cancellationToken = default);
}