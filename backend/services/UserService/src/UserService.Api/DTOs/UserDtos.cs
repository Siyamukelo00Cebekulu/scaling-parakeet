using System.ComponentModel.DataAnnotations;
using UserService.Api.Models;

namespace UserService.Api.DTOs;

public record RegisterRequest
{
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

public record AuthResponse
{
    public string AccessToken { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
    public DateTimeOffset ExpiresAtUtc { get; init; }
}

public record AuthRequest
{
    [Required]
    public string Username { get; init; } = string.Empty;

    [Required]
    public string Password { get; init; } = string.Empty;
}

public record RefreshRequest
{
    [Required]
    public string RefreshToken { get; init; } = string.Empty;
}

public record UserProfileDto
{
    public Guid Id { get; init; }
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? PhoneNumber { get; init; }
    public string? HomeAddress { get; init; }
    public DateTime? DateOfBirth { get; init; }
    public NotificationPreferences NotificationPreferences { get; init; }
}

public record UpdateProfileRequest
{
    [MaxLength(100)]
    public string? FirstName { get; init; }

    [MaxLength(100)]
    public string? LastName { get; init; }

    [Phone]
    [MaxLength(30)]
    public string? PhoneNumber { get; init; }

    [MaxLength(500)]
    public string? HomeAddress { get; init; }

    public DateTime? DateOfBirth { get; init; }

    public NotificationPreferences NotificationPreferences { get; init; }
}

public record UpdateCredentialsRequest
{
    [MaxLength(80)]
    public string? Username { get; init; }

    [MinLength(8)]
    public string? NewPassword { get; init; }

    [Required]
    public string CurrentPassword { get; init; } = string.Empty;
}
