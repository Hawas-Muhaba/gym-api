using GymSystem.Domain.Common;

namespace GymSystem.Domain.Entities;

public class Notification : AuditableEntity
{
    public int AppointmentId { get; set; }
    public Appointment Appointment { get; set; } = null!;

    public string Message {get; set;} = string.Empty;

    public bool Sent {get; set;} 
}