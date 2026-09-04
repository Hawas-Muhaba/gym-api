using GymSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
// public int AppointmentId { get; set; }
//     public Appointment Appointment { get; set; } = null!;

//     public string Message {get; set;} = string.Empty;

//     public bool Sent {get; set;} 
namespace GymSystem.Infrastructure.Persistence.Configurations;
public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(n => n.Id);
        builder.Property(n => n.Message)
            .IsRequired()
            .HasMaxLength(500);
        builder.Property(n => n.Sent)
            .IsRequired();
        // builder.HasOne(n => n.Appointment)
        //     .WithMany()
        //     .HasForeignKey(n => n.AppointmentId);
    }
}