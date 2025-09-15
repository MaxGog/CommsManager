using CommsManager.Models;
using Microsoft.EntityFrameworkCore;

namespace CommsManager.Services.Database;

public class DatabaseContext : DbContext
{
    public DbSet<Commission> Commissions { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<PortfolioItem> PortfolioItems { get; set; }
    public DbSet<PricePreset> PricePresets { get; set; }
    public DbSet<Artwork> Artworks { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<CommissionTag> CommissionTags { get; set; }
    public DbSet<PortfolioTag> PortfolioTags { get; set; }
    public DbSet<AnalyticsRecord> AnalyticsRecords { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CommissionTag>()
            .HasKey(ct => new { ct.CommissionId, ct.TagId });

        modelBuilder.Entity<PortfolioTag>()
            .HasKey(pt => new { pt.PortfolioItemId, pt.TagId });

        modelBuilder.Entity<Commission>()
            .HasIndex(c => c.Status);
            
        modelBuilder.Entity<Commission>()
            .HasIndex(c => c.Deadline);
            
        modelBuilder.Entity<PortfolioItem>()
            .HasIndex(p => p.ArtType);
            
        modelBuilder.Entity<AnalyticsRecord>()
            .HasIndex(a => new { a.Year, a.Month })
            .IsUnique();
    }
}