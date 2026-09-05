namespace GymSystem.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? Role { get; }
    bool IsInRole(string role);
}
