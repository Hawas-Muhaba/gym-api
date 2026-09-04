using GymSystem.Domain.Common;
namespace GymSystem.Domain.Entities;
public class Staff : AuditableEntity
{
    public string? UserId { get; set; }
    public string FullName {get; set;} = string.Empty;
    public string Phone {get; set;} = string.Empty;
    public decimal CommissionRate {get; set;} = 0.0m;
    public ICollection<Appointment> Appointments {get; set;} = new List<Appointment>();
}