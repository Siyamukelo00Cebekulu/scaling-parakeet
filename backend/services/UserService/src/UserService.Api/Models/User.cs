using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Api.Models;

[Flags]
public enum NotificationPreferences
{
    None = 0,
    Email = 1,
    Marketing = 2
}

public class User
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(80)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? FirstName { get; set; }

    [MaxLength(100)]
    public string? LastName { get; set; }

    [Phone]
    [MaxLength(30)]
    public string? PhoneNumber { get; set; }

    [MaxLength(500)]
    public string? HomeAddress { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public NotificationPreferences NotificationPreferences { get; set; } = NotificationPreferences.None;

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset UpdatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    // Navigation: refresh tokens
    public List<RefreshToken> RefreshTokens { get; set; } = new();
}