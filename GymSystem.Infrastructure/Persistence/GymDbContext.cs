using GymSystem.Domain.Common;
using GymSystem.Domain.Entities;
using GymSystem.Application.Common.Interfaces;
using GymSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Infrastructure.Persistence;
public class GymDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public GymDbContext(DbContextOptions<GymDbContext> options) : base(options){ }
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Staff> Staffs => Set<Staff>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<Membership> Memberships => Set<Membership>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymDbContext).Assembly);
        modelBuilder.Entity<Client>().HasQueryFilter(c => !c.IsDeleted);
        modelBuilder.Entity<Staff>().HasQueryFilter(s => !s.IsDeleted);
        modelBuilder.Entity<Appointment>().HasQueryFilter(a => !a.IsDeleted);
        modelBuilder.Entity<Service>().HasQueryFilter(s => !s.IsDeleted);
        modelBuilder.Entity<Membership>().HasQueryFilter(m => !m.IsDeleted);
        modelBuilder.Entity<Payment>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<Notification>().HasQueryFilter(n => !n.IsDeleted);
    }

}