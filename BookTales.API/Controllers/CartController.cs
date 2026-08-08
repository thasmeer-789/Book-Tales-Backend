using BookTales.Application.DTOs.Cart;
using BookTales.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookTales.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyCart()
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized();

        var response = await _cartService.GetMyCartAsync(userId.Value);

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(
        AddCartItemDto request)
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized();

        var result = await _cartService.AddToCartAsync(
            userId.Value,
            request);

        if (!result)
        {
            return BadRequest(new
            {
                success = false,
                message = "Quantity must be greater than zero."
            });
        }

        return Ok(new
        {
            success = true,
            message = "Book added to cart successfully."
        });
    }

    [HttpPut("{bookId}")]
    public async Task<IActionResult> UpdateCartItem(
        Guid bookId,
        UpdateCartItemDto request)
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized();

        var result = await _cartService.UpdateCartItemAsync(
            userId.Value,
            bookId,
            request);

        if (!result)
        {
            return BadRequest(new
            {
                success = false,
                message = "Cart item not found or quantity is invalid."
            });
        }

        return Ok(new
        {
            success = true,
            message = "Cart item updated successfully."
        });
    }

    [HttpDelete("{bookId}")]
    public async Task<IActionResult> RemoveFromCart(Guid bookId)
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized();

        var result = await _cartService.RemoveFromCartAsync(
            userId.Value,
            bookId);

        if (!result)
        {
            return NotFound(new
            {
                success = false,
                message = "Book was not found in the cart."
            });
        }

        return Ok(new
        {
            success = true,
            message = "Book removed from cart successfully."
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