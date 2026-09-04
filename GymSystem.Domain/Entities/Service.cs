using GymSystem.Domain.Common;
namespace GymSystem.Domain.Entities;
public class Service : AuditableEntity
{
    public string Name {get; set;} = string.Empty;
    public decimal Price {get; set;}
    public int DurationMinutes {get; set;}
}
