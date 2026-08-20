using BookTales.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookTales.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();

        return Ok(new
        {
            success = true,
            data = users
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);

        if (user == null)
        {
            return NotFound(new
            {
                success = false,
                message = "User not found."
            });
        }

        return Ok(new
        {
            success = true,
            data = user
        });
    }

    [HttpPut("{id}/block")]
    public async Task<IActionResult> BlockUser(Guid id)
    {
        var currentUserIdClaim =
            User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(currentUserIdClaim, out var currentUserId))
            return Unauthorized();

        try
        {
            var result = await _userService.BlockUserAsync(
                id,
                currentUserId);

            if (!result)
            {
                return NotFound(new
                {
                    success = false,
                    message = "User not found."
                });
            }

            return Ok(new
            {
                success = true,
                message = "User blocked successfully."
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
    }

    [HttpPut("{id}/unblock")]
    public async Task<IActionResult> UnblockUser(Guid id)
    {
        var result = await _userService.UnblockUserAsync(id);

        if (!result)
        {
            return NotFound(new
            {
                success = false,
                message = "User not found."
            });
        }

        return Ok(new
        {
            success = true,
            message = "User unblocked successfully."
        });
    }
}