using Booking.Domain.Entities;
using Booking.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Infrastructure.Data.Configurations;

public class AppointmentEntityConfiguration : IEntityTypeConfiguration<AppointmentEntity>
{
    public void Configure(EntityTypeBuilder<AppointmentEntity> builder)
    {
        builder.ToTable("Appointments");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.ClientName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.StartsAt)
            .IsRequired();

        builder.Property(a => a.EndsAt)
            .IsRequired();

        builder.Property(a => a.Status)
            .IsRequired()
            .HasDefaultValue(AppointmentStatus.Pending)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(a => a.CreatedAt)
            .IsRequired();

        builder.HasQueryFilter(a => a.DeletedAt == null);

        builder.HasOne(a => a.Stylist)
            .WithMany(s => s.Appointments)
            .HasForeignKey(a => a.StylistId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Service)
            .WithMany(s => s.Appointments)
            .HasForeignKey(a => a.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(a => a.StylistId);
        builder.HasIndex(a => a.ServiceId);
        builder.HasIndex(a => a.StartsAt);
        builder.HasIndex(a => a.Status);
    }
}
