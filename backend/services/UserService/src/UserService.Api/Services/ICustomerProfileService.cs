using UserService.Api.DTOs;

namespace UserService.Api.Services;

public interface ICustomerProfileService
{
    Task<UserProfileDto?> GetProfileAsync(Guid userId, CancellationToken ct = default);
    Task UpdateProfileAsync(Guid userId, UpdateProfileRequest request, CancellationToken ct = default);
    Task UpdateCredentialsAsync(Guid userId, UpdateCredentialsRequest request, CancellationToken ct = default);
}
