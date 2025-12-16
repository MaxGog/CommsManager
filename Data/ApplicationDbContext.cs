using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CommsManager.Models;

namespace CommsManager.Data;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Service> Services { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Income> Incomes { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<MaterialInventory> Materials { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Service>()
            .HasIndex(s => s.UserId);

        builder.Entity<Client>()
            .HasIndex(c => c.UserId);

        builder.Entity<Order>()
            .HasIndex(o => o.UserId);

        builder.Entity<Order>()
            .HasIndex(o => o.Status);

        builder.Entity<Income>()
            .HasIndex(i => i.UserId);

        builder.Entity<Income>()
            .HasIndex(i => i.Date);

        builder.Entity<Expense>()
            .HasIndex(e => e.UserId);

        builder.Entity<Expense>()
            .HasIndex(e => e.Date);

        builder.Entity<ApplicationUser>()
            .HasMany(u => u.Services)
            .WithOne(s => s.User)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ApplicationUser>()
            .HasMany(u => u.Clients)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ApplicationUser>()
            .HasMany(u => u.Orders)
            .WithOne(o => o.User)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ApplicationUser>()
            .HasMany(u => u.Incomes)
            .WithOne(i => i.User)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ApplicationUser>()
            .HasMany(u => u.Expenses)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Order>()
            .HasOne(o => o.Client)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Order>()
            .HasOne(o => o.Service)
            .WithMany(s => s.Orders)
            .HasForeignKey(o => o.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}