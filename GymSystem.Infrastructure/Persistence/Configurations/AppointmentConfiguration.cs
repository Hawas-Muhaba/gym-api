using GymSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace GymSystem.Infrastructure.Persistence.Configurations;
public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(a => a.Id);
        builder.HasOne(a=>a.Client)
            .WithMany(s=> s.Appointments)
            .HasForeignKey(a=> a.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(a=>a.Staff)
            .WithMany(s=> s.Appointments)
            .HasForeignKey(a=> a.StaffId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(a=>a.Service)
            .WithMany()
            .HasForeignKey(a=> a.ServiceId);

        builder.Property(a=> a.Status)
            .HasConversion<string>();
        builder.HasIndex(a => new { a.StaffId, a.StartTime })
           .IsUnique()
           .HasFilter("\"Status\" <> 'Cancelled'");
    }
}