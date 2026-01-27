using System.Text.Json;
using CommsManager.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommsManager.Infrastructure.Data.Configurations;

public class ArtistProfileConfiguration : IEntityTypeConfiguration<ArtistProfile>
{
    public void Configure(EntityTypeBuilder<ArtistProfile> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Description)
            .HasMaxLength(2000);

        builder.Property(a => a.CreatedDate)
            .IsRequired();

        builder.Property(a => a.ArtistPicture)
            .HasColumnType("varbinary(max)");

        builder.Property(a => a.ArtistBanner)
            .HasColumnType("varbinary(max)");

        builder.OwnsMany(a => a.Phones, p =>
        {
            p.WithOwner().HasForeignKey("ArtistProfileId");
            p.Property<int>("Id").ValueGeneratedOnAdd();
            p.HasKey("Id");
            p.Property(pp => pp.NumberPhone).IsRequired().HasMaxLength(50);
            p.Property(pp => pp.TypePhone).HasMaxLength(50);
            p.Property(pp => pp.RegionNumber).HasMaxLength(10);
            p.Property(pp => pp.Description).HasMaxLength(200);

            p.HasIndex(pp => pp.NumberPhone);
        });

        builder.OwnsMany(a => a.Emails, e =>
        {
            e.WithOwner().HasForeignKey("ArtistProfileId");
            e.Property<int>("Id").ValueGeneratedOnAdd();
            e.HasKey("Id");
            e.Property(ee => ee.EmailAdress).IsRequired().HasMaxLength(150);
            e.Property(ee => ee.TypeEmail).HasMaxLength(50);
            e.Property(ee => ee.Description).HasMaxLength(200);

            e.HasIndex(ee => ee.EmailAdress);
        });

        builder.OwnsMany(a => a.SocialLinks, s =>
        {
            s.WithOwner().HasForeignKey("ArtistProfileId");
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

        builder.OwnsMany(a => a.Commissions, c =>
        {
            c.WithOwner().HasForeignKey("ArtistProfileId");
            c.Property<int>("Id").ValueGeneratedOnAdd();
            c.HasKey("Id");

            c.Property(cc => cc.Name).IsRequired().HasMaxLength(200);
            c.Property(cc => cc.Description).HasMaxLength(1000);
            c.Property(cc => cc.TypeCommission).HasMaxLength(100);
            c.Property(cc => cc.Price).HasMaxLength(100);

            c.Property(cc => cc.ViewAttachment)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<List<byte[]>>(v, (JsonSerializerOptions?)null))
                .HasColumnType("nvarchar(max)");

            c.HasIndex(cc => cc.Name);
            c.HasIndex(cc => cc.TypeCommission);
        });

        builder.HasIndex(a => a.Name);
        builder.HasIndex(a => a.CreatedDate);
    }
}