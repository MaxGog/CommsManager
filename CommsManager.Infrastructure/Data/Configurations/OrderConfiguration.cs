using CommsManager.Core.Entities;
using CommsManager.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommsManager.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Title).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Description).HasMaxLength(2000);
        builder.Property(e => e.Price).HasConversion(
            v => v.ToString(),
            v => Money.FromString(v)
        );
        builder.Property(e => e.CreatedDate).IsRequired();
        builder.Property(e => e.Deadline).IsRequired();
        builder.Property(e => e.Status).HasConversion<string>().IsRequired();
        builder.Property(e => e.IsActive).IsRequired();

        builder.OwnsMany(e => e.Attachments, a =>
        {
            a.WithOwner().HasForeignKey("OrderId");
            a.Property<int>("Id").ValueGeneratedOnAdd();
            a.HasKey("Id");
            a.Property(aa => aa.Name).HasMaxLength(300);
            a.Property(aa => aa.Attachment).IsRequired();
            a.Property(aa => aa.TypeAttachment).HasConversion<string>().HasMaxLength(50);
            a.Property(aa => aa.Description).HasMaxLength(500);
        });

        builder.HasIndex(e => e.CustomerId);
        builder.HasIndex(e => e.ArtistId);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.Deadline);
        builder.HasIndex(e => e.IsActive);
    }
}