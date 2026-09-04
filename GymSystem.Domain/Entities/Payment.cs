using  GymSystem.Domain.Common;
namespace GymSystem.Domain.Entities;

public class Payment : AuditableEntity
{
    public int AppointmentId {get; set;}
    public Appointment Appointment {get; set;} = null!;

    public decimal Amount {get; set;}

    public decimal StaffCommissionAmount {get; set;}

    public DateTime PaidAt {get; set;} = DateTime.UtcNow;
    
}