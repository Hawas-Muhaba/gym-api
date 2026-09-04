using GymSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymSystem.Infrastructure.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p=>p.Id);
        builder.Property(p=>p.Amount).HasPrecision(10,2);
        builder.Property(p=>p.StaffCommissionAmount).HasPrecision(10,2);
        builder.HasOne(p => p.Appointment)
            .WithOne()
            .HasForeignKey<Payment>(p => p.AppointmentId);
    }
}