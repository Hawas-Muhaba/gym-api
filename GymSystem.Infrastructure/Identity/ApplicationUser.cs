using GymSystem.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace GymSystem.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public UserRole Role { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
}