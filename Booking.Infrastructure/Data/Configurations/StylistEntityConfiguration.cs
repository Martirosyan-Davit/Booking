using Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Infrastructure.Data.Configurations;

public class StylistEntityConfiguration : IEntityTypeConfiguration<StylistEntity>
{
    public void Configure(EntityTypeBuilder<StylistEntity> builder)
    {
        builder.ToTable("Stylists");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Specialization)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.WorkStart)
            .IsRequired();

        builder.Property(s => s.WorkEnd)
            .IsRequired();

        builder.Property(s => s.CreatedAt)
            .IsRequired();
    }
}
