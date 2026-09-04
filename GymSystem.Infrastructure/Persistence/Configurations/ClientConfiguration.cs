using GymSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymSystem.Infrastructure.Persistence.Configurations;


public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.FullName)
            .IsRequired()
            .HasMaxLength(150);
        builder.Property(c => c.Phone).IsRequired().HasMaxLength(20);

        builder.HasMany(c => c.Memberships)
            .WithOne(m => m.Client)
            .HasForeignKey(m => m.ClientId);
    }
}