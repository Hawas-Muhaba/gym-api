namespace GymSystem.Application.Common.Interfaces;
public record TokenResult(string AccessToken, string RefreshToken, DateTime AccessTokenExpiry);
public interface IIdentityService
{
    Task<(bool Succeeded, string? UserId, string[] Errors)> RegisterAsync(string email, string password, string role);
    Task<(bool Succeeded, TokenResult? Tokens)> LoginAsync(string email, string password);
    Task<(bool Succeeded, TokenResult? Tokens)> RefreshAsync(string expiredAccessToken, string refreshToken);}