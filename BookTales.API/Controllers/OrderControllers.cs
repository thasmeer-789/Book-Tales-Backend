using BookTales.Application.DTOs.Orders;
using BookTales.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookTales.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            dto.UserId = userId.Value;

            var order = await _orderService.CreateOrderAsync(dto);

            return Ok(new
            {
                success = true,
                message = "Order created successfully.",
                data = order
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var orders = await _orderService.GetMyOrdersAsync(userId.Value);

            return Ok(new
            {
                success = true,
                data = orders
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var userId = GetUserId();

            if (userId == null)
                return Unauthorized();

            var order = await _orderService.GetOrderByIdAsync(
                id,
                userId.Value);

            if (order == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Order not found."
                });
            }

            return Ok(new
            {
                success = true,
                data = order
            });
        }

        [Authorize(
      AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
      Roles = "Admin")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(
      Guid id,
      UpdateOrderStatusDto dto)
        {
            try
            {
                var order = await _orderService.UpdateOrderStatusAsync(
                    id,
                    dto);

                if (order == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Order not found."
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Order status updated successfully.",
                    data = order
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

        [Authorize(
    AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
    Roles = "Admin")]
        [HttpPut("{id}/payment-status")]
        public async Task<IActionResult> UpdatePaymentStatus(
    Guid id,
    UpdatePaymentStatusDto dto)
        {
            try
            {
                var order = await _orderService.UpdatePaymentStatusAsync(
                    id,
                    dto);

                if (order == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Order not found."
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Payment status updated successfully.",
                    data = order
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

        private Guid? GetUserId()
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdClaim, out var userId))
                return null;

            return userId;
        }
    }

}