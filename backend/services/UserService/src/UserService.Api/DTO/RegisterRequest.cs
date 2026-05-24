using System.ComponentModel.DataAnnotations;

namespace UserService.Api.DTO;

public class RegisterRequest
{
    [Required]
    [MaxLength(80)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;
}