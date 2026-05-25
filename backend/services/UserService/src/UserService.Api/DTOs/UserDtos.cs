using System.ComponentModel.DataAnnotations;
using UserService.Api.Models;

namespace UserService.Api.DTOs;

/// <summary>
/// Request payload to register a new user.
/// </summary>
public record RegisterRequest
{
    /// <summary>
    /// Desired username for the new account.
    /// </summary>
    [Required]
    [MaxLength(80)]
    public string Username { get; init; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(200)]
    public string Email { get; init; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; init; } = string.Empty;
}

/// <summary>
/// Response containing access and refresh tokens after successful authentication.
/// </summary>
public record AuthResponse
{
    /// <summary>
    /// JWT access token.
    /// </summary>
    public string AccessToken { get; init; } = string.Empty;

    /// <summary>
    /// Refresh token used to obtain a new access token.
    /// </summary>
    public string RefreshToken { get; init; } = string.Empty;

    /// <summary>
    /// Expiration date and time for the access token.
    /// </summary>
    public DateTimeOffset ExpiresAtUtc { get; init; }
}

/// <summary>
/// Request payload used to authenticate a user.
/// </summary>
public record AuthRequest
{
    /// <summary>
    /// Username of the authenticating user.
    /// </summary>
    [Required]
    public string Username { get; init; } = string.Empty;

    [Required]
    public string Password { get; init; } = string.Empty;
}

/// <summary>
/// Request payload for refreshing authentication tokens.
/// </summary>
public record RefreshRequest
{
    /// <summary>
    /// Current refresh token to exchange for a new access token.
    /// </summary>
    [Required]
    public string RefreshToken { get; init; } = string.Empty;
}

/// <summary>
/// Profile data returned to the client for the authenticated user.
/// </summary>
public record UserProfileDto
{
    /// <summary>
    /// User identifier.
    /// </summary>
    public Guid Id { get; init; }
    /// <summary>
    /// Username of the user.
    /// </summary>
    public string Username { get; init; } = string.Empty;

    /// <summary>
    /// Email address of the user.
    /// </summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// User first name.
    /// </summary>
    public string? FirstName { get; init; }

    /// <summary>
    /// User last name.
    /// </summary>
    public string? LastName { get; init; }

    /// <summary>
    /// User phone number.
    /// </summary>
    public string? PhoneNumber { get; init; }

    /// <summary>
    /// Home address for the user.
    /// </summary>
    public string? HomeAddress { get; init; }

    /// <summary>
    /// User date of birth.
    /// </summary>
    public DateTime? DateOfBirth { get; init; }

    /// <summary>
    /// Notification preferences for the user.
    /// </summary>
    public NotificationPreferences NotificationPreferences { get; init; }
}

/// <summary>
/// Request payload for updating a user's profile fields.
/// </summary>
public record UpdateProfileRequest
{
    /// <summary>
    /// Optional first name update.
    /// </summary>
    [MaxLength(100)]
    public string? FirstName { get; init; }

    [MaxLength(100)]
    public string? LastName { get; init; }

    [Phone]
    [MaxLength(30)]
    public string? PhoneNumber { get; init; }

    /// <summary>
    /// Optional home address update.
    /// </summary>
    [MaxLength(500)]
    public string? HomeAddress { get; init; }

    /// <summary>
    /// Optional date of birth update.
    /// </summary>
    public DateTime? DateOfBirth { get; init; }

    /// <summary>
    /// Notification preference updates.
    /// </summary>
    public NotificationPreferences NotificationPreferences { get; init; }
}

/// <summary>
/// Request payload for updating user credentials.
/// </summary>
public record UpdateCredentialsRequest
{
    /// <summary>
    /// Optional new username to replace the existing one.
    /// </summary>
    [MaxLength(80)]
    public string? Username { get; init; }

    /// <summary>
    /// Optional new password for the user.
    /// </summary>
    [MinLength(8)]
    public string? NewPassword { get; init; }

    /// <summary>
    /// Current password required to authorize credential changes.
    /// </summary>
    [Required]
    public string CurrentPassword { get; init; } = string.Empty;
}
