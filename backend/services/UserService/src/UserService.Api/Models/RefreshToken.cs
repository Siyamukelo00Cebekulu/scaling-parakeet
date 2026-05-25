using System.ComponentModel.DataAnnotations;

namespace UserService.Api.Models;

public class RefreshToken
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Token { get; set; } = string.Empty;

    public DateTimeOffset ExpiresAtUtc { get; set; }

    public bool IsRevoked { get; set; } = false;

    public Guid UserId { get; set; }

    public User? User { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
}
