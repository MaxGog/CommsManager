using CommsManager.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommsManager.Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Description)
            .HasMaxLength(1000);

        builder.Property(c => c.Communication)
            .HasMaxLength(500);

        builder.Property(c => c.CreatedDate)
            .IsRequired();

        builder.Property(c => c.IsActive)
            .IsRequired();

        builder.Property(c => c.CustomerPicture)
            .HasColumnType("varbinary(max)");

        builder.OwnsMany(c => c.Phones, p =>
        {
            p.WithOwner().HasForeignKey("CustomerId");
            p.Property<int>("Id").ValueGeneratedOnAdd();
            p.HasKey("Id");
            p.Property(pp => pp.NumberPhone).IsRequired().HasMaxLength(50);
            p.Property(pp => pp.TypePhone).HasMaxLength(50);
            p.Property(pp => pp.RegionNumber).HasMaxLength(10);
            p.Property(pp => pp.Description).HasMaxLength(200);

            p.HasIndex(pp => pp.NumberPhone);
        });

        builder.OwnsMany(c => c.Emails, e =>
        {
            e.WithOwner().HasForeignKey("CustomerId");
            e.Property<int>("Id").ValueGeneratedOnAdd();
            e.HasKey("Id");
            e.Property(ee => ee.EmailAdress).IsRequired().HasMaxLength(150);
            e.Property(ee => ee.TypeEmail).HasMaxLength(50);
            e.Property(ee => ee.Description).HasMaxLength(200);

            e.HasIndex(ee => ee.EmailAdress);
        });

        builder.OwnsMany(c => c.SocialLinks, s =>
        {
            s.WithOwner().HasForeignKey("CustomerId");
            s.Property<int>("Id").ValueGeneratedOnAdd();
            s.HasKey("Id");
            s.Property(ss => ss.Link).IsRequired().HasMaxLength(500);
            s.Property(ss => ss.TypeLink).HasConversion<string>().HasMaxLength(50);
            s.Property(ss => ss.IsActive).IsRequired();
            s.Property(ss => ss.IsVisible).IsRequired();

            s.HasIndex(ss => ss.TypeLink);
            s.HasIndex(ss => ss.IsActive);
            s.HasIndex(ss => ss.IsVisible);
        });

        builder.HasMany(c => c.Orders)
            .WithOne()
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => c.Name);
        builder.HasIndex(c => c.CreatedDate);
        builder.HasIndex(c => c.IsActive);
        builder.HasIndex(c => new { c.IsActive, c.Name });
    }
}