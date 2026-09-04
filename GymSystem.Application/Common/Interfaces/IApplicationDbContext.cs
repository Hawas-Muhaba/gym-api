using  GymSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Application.Common.Interfaces;
public interface IApplicationDbContext
{
    DbSet<Client> Clients {get;}
    DbSet<Staff> Staffs {get;}
    DbSet<Service> Services {get;}
    DbSet<Membership> Memberships {get;}
    DbSet<Appointment> Appointments {get;}
    DbSet<Payment> Payments {get;}
    DbSet<Notification> Notifications {get;}

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}