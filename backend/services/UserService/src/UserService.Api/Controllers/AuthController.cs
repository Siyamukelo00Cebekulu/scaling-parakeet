using Microsoft.AspNetCore.Mvc;
using UserService.Api.DTOs;
using UserService.Api.Services;

namespace UserService.Api.Controllers;

/// <summary>
/// Handles authentication and token lifecycle endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IIdentityService _identity;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    public AuthController(IIdentityService identity)
    {
        _identity = identity;
    }

    /// <summary>
    /// Registers a new user and returns authentication tokens.
    /// </summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _identity.RegisterAsync(request);
        return CreatedAtAction(nameof(Register), result);
    }

    /// <summary>
    /// Authenticates an existing user and returns a JWT access token.
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthRequest request)
    {
        var result = await _identity.AuthenticateAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Exchanges a refresh token for a new access token.
    /// </summary>
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
    {
        var result = await _identity.RefreshAsync(request);
        return Ok(result);
    }
}