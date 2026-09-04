using GymSystem.Domain.Common;
namespace GymSystem.Domain.Entities;
using GymSystem.Domain.Enums;
public class Appointment : AuditableEntity
{
    public int ClientId {get; set;}
    public Client Client {get; set;} = null!;
    public int StaffId {get; set;}
    public Staff Staff {get; set;} = null!;
    public int ServiceId {get; set;}
    public Service Service {get; set;} = null!;
    public DateTime StartTime {get; set;}
    public DateTime EndTime {get; set;}
    public BookingStatus Status {get; set;} = BookingStatus.Pending;
}