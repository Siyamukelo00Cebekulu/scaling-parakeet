using Microsoft.AspNetCore.Identity;
using UserService.Api.DTOs;
using UserService.Api.Models;
using UserService.Api.Repositories;

namespace UserService.Api.Services;

/// <summary>
/// Handles profile reads and updates for authenticated users.
/// </summary>
public class CustomerProfileService : ICustomerProfileService
{
    private readonly IUserRepository _repo;
    private readonly IPasswordHasher<User> _passwordHasher;

    public CustomerProfileService(IUserRepository repo, IPasswordHasher<User> passwordHasher)
    {
        _repo = repo;
        _passwordHasher = passwordHasher;
    }

    /// <inheritdoc/>
    public async Task<UserProfileDto?> GetProfileAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await _repo.GetByIdAsync(userId, ct);
        if (user == null) return null;

        return new UserProfileDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            HomeAddress = user.HomeAddress,
            DateOfBirth = user.DateOfBirth,
            NotificationPreferences = user.NotificationPreferences
        };
    }

    /// <inheritdoc/>
    public async Task UpdateProfileAsync(Guid userId, UpdateProfileRequest request, CancellationToken ct = default)
    {
        var user = await _repo.GetByIdAsync(userId, ct);
        if (user == null) throw new KeyNotFoundException("User not found");

        user.FirstName = request.FirstName ?? user.FirstName;
        user.LastName = request.LastName ?? user.LastName;
        user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;
        user.HomeAddress = request.HomeAddress ?? user.HomeAddress;
        user.DateOfBirth = request.DateOfBirth ?? user.DateOfBirth;
        user.NotificationPreferences = request.NotificationPreferences;
        user.UpdatedAtUtc = DateTimeOffset.UtcNow;

        await _repo.UpdateAsync(user, ct);
        await _repo.SaveChangesAsync(ct);
    }

    /// <inheritdoc/>
    public async Task UpdateCredentialsAsync(Guid userId, UpdateCredentialsRequest request, CancellationToken ct = default)
    {
        var user = await _repo.GetByIdAsync(userId, ct);
        if (user == null) throw new KeyNotFoundException("User not found");

        var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.CurrentPassword);
        if (verify == PasswordVerificationResult.Failed) throw new UnauthorizedAccessException("Current password is incorrect");

        if (!string.IsNullOrWhiteSpace(request.Username)) user.Username = request.Username;
        if (!string.IsNullOrWhiteSpace(request.NewPassword))
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
        }

        user.UpdatedAtUtc = DateTimeOffset.UtcNow;

        await _repo.UpdateAsync(user, ct);
        await _repo.SaveChangesAsync(ct);
    }
}
