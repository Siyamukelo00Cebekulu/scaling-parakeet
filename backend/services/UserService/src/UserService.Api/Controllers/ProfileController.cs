using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Api.DTOs;
using UserService.Api.Services;

namespace UserService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly ICustomerProfileService _profileService;

    public ProfileController(ICustomerProfileService profileService)
    {
        _profileService = profileService;
    }

    private Guid GetUserId()
    {
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Name) ?? User.FindFirstValue("sub");
        if (Guid.TryParse(sub, out var id)) return id;
        throw new InvalidOperationException("Unable to determine user id from token");
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var id = GetUserId();
        var profile = await _profileService.GetProfileAsync(id);
        if (profile == null) return NotFound();
        return Ok(profile);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateProfileRequest request)
    {
        var id = GetUserId();
        await _profileService.UpdateProfileAsync(id, request);
        return NoContent();
    }

    [HttpPut("credentials")]
    public async Task<IActionResult> UpdateCredentials([FromBody] UpdateCredentialsRequest request)
    {
        var id = GetUserId();
        await _profileService.UpdateCredentialsAsync(id, request);
        return NoContent();
    }
}
