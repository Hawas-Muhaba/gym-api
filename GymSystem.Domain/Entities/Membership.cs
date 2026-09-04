using GymSystem.Domain.Common;
namespace GymSystem.Domain.Entities;

public class Membership : AuditableEntity
{
    public int ClientId {get; set;}
    public Client Client {get; set;} = null!;
    public DateTime StartDate {get; set;}
    public DateTime ExpiryDate {get; set;}

    public bool IsActive => ExpiryDate >= DateTime.UtcNow;

}