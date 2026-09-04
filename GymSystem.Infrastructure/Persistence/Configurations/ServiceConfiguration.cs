using GymSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymSystem.Infrastructure.Persistence.Configurations;
public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    // public string Name {get; set;} = string.Empty;
    // public decimal Price {get; set;}
    // public TimeSpan DurationMinutes {get; set;}
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(s => s.Price).HasPrecision(10, 2);
        builder.Property(s => s.DurationMinutes).IsRequired();
    }
}