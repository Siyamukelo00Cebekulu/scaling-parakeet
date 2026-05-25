using System.ComponentModel.DataAnnotations;

namespace UserService.Api.Models;

/// <summary>
/// Stores a refresh token linked to a user for token renewal.
/// </summary>
public class RefreshToken
{
    /// <summary>
    /// Primary key for the refresh token.
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// The opaque refresh token.
    /// </summary>
    public string Token { get; set; } = string.Empty;

    public DateTimeOffset ExpiresAtUtc { get; set; }

    public bool IsRevoked { get; set; } = false;

    public Guid UserId { get; set; }

    public User? User { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}
