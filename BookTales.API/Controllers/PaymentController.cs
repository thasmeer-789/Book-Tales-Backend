using BookTales.Application.DTOs.Payment;
using BookTales.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookTales.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("create-order")]
    public async Task<IActionResult> CreatePaymentOrder(
        [FromBody] CreatePaymentOrderDto dto)
    {
        var userId = GetUserId();

        if (userId == null)
        {
            return Unauthorized("Invalid user token.");
        }

        try
        {
            var result =
                await _paymentService.CreatePaymentOrderAsync(
                    dto,
                    userId.Value);

            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("verify")]
    public async Task<IActionResult> VerifyPayment(
        [FromBody] VerifyPaymentDto dto)
    {
        var userId = GetUserId();

        if (userId == null)
        {
            return Unauthorized("Invalid user token.");
        }

        try
        {
            var isVerified =
                await _paymentService.VerifyPaymentAsync(
                    dto,
                    userId.Value);

            if (!isVerified)
            {
                return BadRequest(new
                {
                    message = "Payment verification failed."
                });
            }

            return Ok(new
            {
                message = "Payment verified successfully.",
                orderId = dto.OrderId,
                paymentStatus = "Paid"
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("fail")]
    public async Task<IActionResult> MarkPaymentFailed(
        [FromBody] CreatePaymentOrderDto dto)
    {
        var userId = GetUserId();

        if (userId == null)
        {
            return Unauthorized("Invalid user token.");
        }

        try
        {
            await _paymentService.MarkPaymentFailedAsync(
                dto.OrderId,
                userId.Value);

            return Ok(new
            {
                message = "Order marked as payment failed."
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
    }

    private Guid? GetUserId()
    {
        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (Guid.TryParse(userIdClaim, out var userId))
        {
            return userId;
        }

        return null;
    }
}