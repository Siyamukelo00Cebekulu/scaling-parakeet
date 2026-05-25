using UserService.Api.DTOs;

namespace UserService.Api.Services;

/// <summary>
/// Defines customer profile read and update operations.
/// </summary>
public interface ICustomerProfileService
{
    /// <summary>
    /// Retrieves the profile for the specified user.
    /// </summary>
    Task<UserProfileDto?> GetProfileAsync(Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Updates profile fields for the specified user.
    /// </summary>
    Task UpdateProfileAsync(Guid userId, UpdateProfileRequest request, CancellationToken ct = default);

    /// <summary>
    /// Updates credential details for the specified user.
    /// </summary>
    Task UpdateCredentialsAsync(Guid userId, UpdateCredentialsRequest request, CancellationToken ct = default);
}
