using UserService.Api.DTOs;

namespace UserService.Api.Services;

/// <summary>
/// Defines authentication and token management operations for the user service.
/// </summary>
public interface IIdentityService
{
    /// <summary>
    /// Registers a new user and returns authentication tokens.
    /// </summary>
    Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default);

    /// <summary>
    /// Validates credentials and returns tokens for the user.
    /// </summary>
    Task<AuthResponse> AuthenticateAsync(AuthRequest request, CancellationToken ct = default);

    /// <summary>
    /// Refreshes authentication tokens using a refresh token.
    /// </summary>
    Task<AuthResponse> RefreshAsync(RefreshRequest request, CancellationToken ct = default);
}
