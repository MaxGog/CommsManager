using Microsoft.EntityFrameworkCore;
using CommsManager.Core.Entities;
using CommsManager.Core.Interfaces;
using CommsManager.Infrastructure.Data;
using System.Linq.Expressions;

namespace CommsManager.Infrastructure.Repositories;

public class ArtistProfileRepository(ApplicationDbContext context) : IArtistProfileRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<ArtistProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ArtistProfiles
            .Include(a => a.Phones)
            .Include(a => a.Emails)
            .Include(a => a.SocialLinks)
            .Include(a => a.Commissions)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<ArtistProfile>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ArtistProfiles
            .Include(a => a.Phones)
            .Include(a => a.Emails)
            .Include(a => a.Commissions)
            .OrderBy(a => a.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ArtistProfile>> FindAsync(
        Expression<Func<ArtistProfile, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.ArtistProfiles
            .Where(predicate)
            .Include(a => a.Commissions)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ArtistProfile entity, CancellationToken cancellationToken = default)
    {
        await _context.ArtistProfiles.AddAsync(entity, cancellationToken);
    }

    public Task UpdateAsync(ArtistProfile entity, CancellationToken cancellationToken = default)
    {
        _context.ArtistProfiles.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(ArtistProfile entity, CancellationToken cancellationToken = default)
    {
        _context.ArtistProfiles.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ArtistProfiles.AnyAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ArtistProfiles.CountAsync(cancellationToken);
    }

    public async Task<int> CountAsync(
        Expression<Func<ArtistProfile, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.ArtistProfiles.CountAsync(predicate, cancellationToken);
    }

    public async Task<ArtistProfile?> GetProfileWithCommissionsAsync(
        Guid artistId, CancellationToken cancellationToken = default)
    {
        return await _context.ArtistProfiles
            .Include(a => a.Commissions)
            .FirstOrDefaultAsync(a => a.Id == artistId, cancellationToken);
    }

    public async Task<IEnumerable<ArtistProfile>> SearchByNameAsync(
        string name, CancellationToken cancellationToken = default)
    {
        return await _context.ArtistProfiles
            .Where(a => a.Name.Contains(name))
            .Include(a => a.Commissions)
            .OrderBy(a => a.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasActiveCommissionsAsync(
        Guid artistId, CancellationToken cancellationToken = default)
    {
        return await _context.ArtistProfiles
            .AnyAsync(a => a.Id == artistId && a.Commissions.Any(), cancellationToken);
    }

    public async Task<IEnumerable<ArtistProfile>> GetPopularArtistsAsync(
        int count, CancellationToken cancellationToken = default)
    {
        return await _context.ArtistProfiles
            .Include(a => a.Commissions)
            .OrderByDescending(a => a.Commissions.Count)
            .Take(count)
            .ToListAsync(cancellationToken);
    }
}