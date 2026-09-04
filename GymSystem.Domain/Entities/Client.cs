using GymSystem.Domain.Common;
namespace GymSystem.Domain.Entities;

public class Client : AuditableEntity
{
    public string? UserId { get; set; }
    public string FullName {get; set;} = string.Empty;
    public string Email {get; set;} = string.Empty;
    public string Phone {get; set;} = string.Empty;
    public string? Notes {get; set;}
    public string? TelegramChatId { get; set; } 

    //nav properties
    public ICollection<Appointment> Appointments {get; set;} = new List<Appointment>();
    public ICollection<Membership> Memberships {get; set;} = new List<Membership>();
}