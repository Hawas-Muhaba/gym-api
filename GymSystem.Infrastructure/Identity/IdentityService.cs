using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GymSystem.Application.Common.Interfaces;
using GymSystem.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GymSystem.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public IdentityService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<(bool, string?, string[])> RegisterAsync(string email, string password, string role)
    {
        var user = new ApplicationUser { UserName = email, Email = email, Role = Enum.Parse<UserRole>(role) };
        var result = await _userManager.CreateAsync(user, password);
        return result.Succeeded
            ? (true, user.Id, Array.Empty<string>())
            : (false, null, result.Errors.Select(e => e.Description).ToArray());
    }

    public async Task<(bool, TokenResult?)> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null || !await _userManager.CheckPasswordAsync(user, password))
            return (false, null);

        var tokens = await GenerateTokensAsync(user);
        return (true, tokens);
    }

    public async Task<(bool, TokenResult?)> RefreshAsync(string expiredAccessToken, string refreshToken)
    {
        var principal = GetPrincipalFromExpiredToken(expiredAccessToken); // reads claims WITHOUT checking expiry
        if (principal is null) return (false, null);

        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId!);

        // The critical security checks: does the refresh token match what we stored, and hasn't it expired?
        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
            return (false, null);

        var tokens = await GenerateTokensAsync(user); // issue a fresh pair, rotating the refresh token too
        return (true, tokens);
    }

    private async Task<TokenResult> GenerateTokensAsync(ApplicationUser user)
    {
        var accessTokenExpiry = DateTime.UtcNow.AddHours(2);
        var accessToken = GenerateAccessToken(user, accessTokenExpiry);
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user); // persist so RefreshAsync can validate it later

        return new TokenResult(accessToken, refreshToken, accessTokenExpiry);
    }

    private string GenerateAccessToken(ApplicationUser user, DateTime expiry)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiry,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes); // cryptographically random, NOT a JWT — just an opaque, unguessable string
        return Convert.ToBase64String(randomBytes);
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var validationParams = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _configuration["Jwt:Issuer"],
            ValidAudience = _configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!)),
            ValidateLifetime = false, // 👈 the key line: we WANT to read an expired token's claims here
        };

        var handler = new JwtSecurityTokenHandler();
        try
        {
            var principal = handler.ValidateToken(token, validationParams, out var securityToken);
            // still verify it's a genuinely signed, well-formed token — just not checking the expiry date
            if (securityToken is not JwtSecurityToken jwt || !jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                return null;
            return principal;
        }
        catch
        {
            return null; // malformed or tampered token — reject outright
        }
    }
}