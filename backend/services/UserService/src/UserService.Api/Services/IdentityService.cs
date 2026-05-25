using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UserService.Api.DTOs;
using UserService.Api.Models;
using UserService.Api.Repositories;

namespace UserService.Api.Services;

/// <summary>
/// Manages user registration, authentication, and refresh token issuance.
/// </summary>
public class IdentityService : IIdentityService
{
    private readonly IUserRepository _repo;
    private readonly IConfiguration _config;
    private readonly IPasswordHasher<User> _passwordHasher;

    public IdentityService(IUserRepository repo, IConfiguration config, IPasswordHasher<User> passwordHasher)
    {
        _repo = repo;
        _config = config;
        _passwordHasher = passwordHasher;
    }

    /// <inheritdoc/>
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        var existing = await _repo.GetByUsernameAsync(request.Username, ct);
        if (existing != null) throw new InvalidOperationException("Username already exists");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            CreatedAtUtc = DateTimeOffset.UtcNow,
            UpdatedAtUtc = DateTimeOffset.UtcNow
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        await _repo.AddAsync(user, ct);
        await _repo.SaveChangesAsync(ct);

        return await GenerateTokensForUserAsync(user, ct);
    }

    /// <inheritdoc/>
    public async Task<AuthResponse> AuthenticateAsync(AuthRequest request, CancellationToken ct = default)
    {
        var user = await _repo.GetByUsernameAsync(request.Username, ct);
        if (user == null) throw new UnauthorizedAccessException("Invalid username or password");

        var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (verify == PasswordVerificationResult.Failed) throw new UnauthorizedAccessException("Invalid username or password");

        return await GenerateTokensForUserAsync(user, ct);
    }

    /// <inheritdoc/>
    public async Task<AuthResponse> RefreshAsync(RefreshRequest request, CancellationToken ct = default)
    {
        var existing = await _repo.GetRefreshTokenAsync(request.RefreshToken, ct);
        if (existing == null || existing.IsRevoked || existing.ExpiresAtUtc < DateTimeOffset.UtcNow)
            throw new UnauthorizedAccessException("Invalid refresh token");

        // revoke the old token
        await _repo.RevokeRefreshTokenAsync(existing, ct);

        var user = existing.User ?? await _repo.GetByIdAsync(existing.UserId, ct);
        if (user == null) throw new UnauthorizedAccessException("User not found for refresh token");

        var newTokens = await GenerateTokensForUserAsync(user, ct);
        await _repo.SaveChangesAsync(ct);
        return newTokens;
    }

    private async Task<AuthResponse> GenerateTokensForUserAsync(User user, CancellationToken ct)
    {
        var jwtSection = _config.GetSection("Jwt");
        var key = jwtSection.GetValue<string>("Key") ?? throw new InvalidOperationException("Jwt:Key not configured");
        var issuer = jwtSection.GetValue<string>("Issuer") ?? "userservice";
        var audience = jwtSection.GetValue<string>("Audience") ?? "userservice_clients";
        var accessMinutes = jwtSection.GetValue<int?>("AccessTokenExpirationMinutes") ?? 15;
        var refreshDays = jwtSection.GetValue<int?>("RefreshTokenExpirationDays") ?? 7;

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(accessMinutes),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(token);

        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
            ExpiresAtUtc = DateTimeOffset.UtcNow.AddDays(refreshDays),
            UserId = user.Id
        };

        await _repo.AddRefreshTokenAsync(refreshToken, ct);
        await _repo.SaveChangesAsync(ct);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            ExpiresAtUtc = new DateTimeOffset(tokenDescriptor.Expires ?? DateTime.UtcNow.AddMinutes(accessMinutes))
        };
    }
}
