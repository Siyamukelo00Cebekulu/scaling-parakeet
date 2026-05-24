using System.ComponentModel.DataAnnotations;

namespace UserService.Api.DTO;

public class LoginRequest
{
    [Required]
    [MaxLength(80)]
    public string UsernameOrEmail { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}