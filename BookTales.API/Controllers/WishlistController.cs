using BookTales.Application.DTOs.Wishlist;
using BookTales.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookTales.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class WishlistController : ControllerBase
{
    private readonly IWishlistService _wishlistService;

    public WishlistController(IWishlistService wishlistService)
    {
        _wishlistService = wishlistService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyWishlist()
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized();

        var response = await _wishlistService.GetMyWishlistAsync(userId.Value);

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddToWishlist(
    AddWishlistItemDto request)
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized();

        try
        {
            var result = await _wishlistService.AddToWishlistAsync(
                userId.Value,
                request);

            if (!result)
            {
                return Conflict(new
                {
                    success = false,
                    message = "Book is already in the wishlist."
                });
            }

            return Ok(new
            {
                success = true,
                message = "Book added to wishlist successfully."
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

    [HttpDelete("{bookId}")]
    public async Task<IActionResult> RemoveFromWishlist(Guid bookId)
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized();

        var result = await _wishlistService.RemoveFromWishlistAsync(
            userId.Value,
            bookId);

        if (!result)
        {
            return NotFound(new
            {
                success = false,
                message = "Book was not found in the wishlist."
            });
        }

        return Ok(new
        {
            success = true,
            message = "Book removed from wishlist successfully."
        });
    }

    private Guid? GetUserId()
    {
        var userIdClaim = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdClaim, out var userId))
            return null;

        return userId;
    }
}