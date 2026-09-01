using BookTales.Application.DTOs.User;
using BookTales.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookTales.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IUserService _userService;

    public ProfileController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized(new
            {
                success = false,
                message = "Invalid user token."
            });

        var profile =
            await _userService.GetMyProfileAsync(userId.Value);

        if (profile == null)
            return NotFound(new
            {
                success = false,
                message = "User not found."
            });

        return Ok(new
        {
            success = true,
            data = profile
        });
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile(
        [FromBody] UpdateUserProfileDto dto)
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized(new
            {
                success = false,
                message = "Invalid user token."
            });

        if (string.IsNullOrWhiteSpace(dto.FirstName) ||
            string.IsNullOrWhiteSpace(dto.LastName))
        {
            return BadRequest(new
            {
                success = false,
                message = "First name and last name are required."
            });
        }

        var profile =
            await _userService.UpdateMyProfileAsync(
                userId.Value,
                dto);

        if (profile == null)
            return NotFound(new
            {
                success = false,
                message = "User not found."
            });

        return Ok(new
        {
            success = true,
            message = "Profile updated successfully.",
            data = profile
        });
    }

    private Guid? GetUserId()
    {
        var claim =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        if (Guid.TryParse(claim, out var userId))
            return userId;

        return null;
    }
}