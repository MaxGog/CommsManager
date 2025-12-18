using CommsManager.Core.Entities;

namespace CommsManager.Core.Interfaces.Services;

public interface IArtistProfileService
{
    Task<ArtistProfile> CreateProfileAsync(
        Guid userId,
        string displayName,
        string bio,
        CancellationToken cancellationToken = default);

    Task<ArtistProfile?> GetProfileByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<ArtistProfile?> GetProfileByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default);

    Task UpdateProfileAsync(
        Guid profileId,
        string displayName,
        string bio,
        string avatarUrl,
        CancellationToken cancellationToken = default);

    Task AddSocialLinkAsync(
        Guid profileId,
        string platform,
        string url,
        CancellationToken cancellationToken = default);

    Task RemoveSocialLinkAsync(
        Guid profileId,
        Guid linkId,
        CancellationToken cancellationToken = default);

    Task<string> GenerateQrCodeAsync(
        Guid profileId,
        int size = 300,
        CancellationToken cancellationToken = default);

    Task<string> GetPublicProfileUrlAsync(
        Guid profileId,
        CancellationToken cancellationToken = default);
}