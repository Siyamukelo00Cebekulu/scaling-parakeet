using Microsoft.AspNetCore.Mvc;
using UserService.Api.DTOs;
using UserService.Api.Services;

namespace UserService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IIdentityService _identity;

    public AuthController(IIdentityService identity)
    {
        _identity = identity;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _identity.RegisterAsync(request);
        return CreatedAtAction(nameof(Register), result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthRequest request)
    {
        var result = await _identity.AuthenticateAsync(request);
        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
    {
        var result = await _identity.RefreshAsync(request);
        return Ok(result);
    }
}