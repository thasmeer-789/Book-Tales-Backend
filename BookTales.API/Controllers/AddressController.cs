using BookTales.Application.DTOs.Address;
using BookTales.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookTales.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AddressController : ControllerBase
{
    private readonly IAddressService _addressService;

    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    // GET: api/Address
    [HttpGet]
    public async Task<IActionResult> GetMyAddresses()
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized(new
            {
                success = false,
                message = "Invalid user token."
            });

        var addresses =
            await _addressService.GetMyAddressesAsync(userId.Value);

        return Ok(new
        {
            success = true,
            data = addresses
        });
    }

    // GET: api/Address/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAddress(Guid id)
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized(new
            {
                success = false,
                message = "Invalid user token."
            });

        var address =
            await _addressService.GetAddressByIdAsync(
                id,
                userId.Value);

        if (address == null)
            return NotFound(new
            {
                success = false,
                message = "Address not found."
            });

        return Ok(new
        {
            success = true,
            data = address
        });
    }

    // POST: api/Address
    [HttpPost]
    public async Task<IActionResult> CreateAddress(
        [FromBody] CreateUpdateAddressDto dto)
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized(new
            {
                success = false,
                message = "Invalid user token."
            });

        var address =
            await _addressService.CreateAddressAsync(
                userId.Value,
                dto);

        return Ok(new
        {
            success = true,
            message = "Address added successfully.",
            data = address
        });
    }

    // PUT: api/Address/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAddress(
        Guid id,
        [FromBody] CreateUpdateAddressDto dto)
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized(new
            {
                success = false,
                message = "Invalid user token."
            });

        var address =
            await _addressService.UpdateAddressAsync(
                id,
                userId.Value,
                dto);

        if (address == null)
            return NotFound(new
            {
                success = false,
                message = "Address not found."
            });

        return Ok(new
        {
            success = true,
            message = "Address updated successfully.",
            data = address
        });
    }

    // DELETE: api/Address/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAddress(Guid id)
    {
        var userId = GetUserId();

        if (userId == null)
            return Unauthorized(new
            {
                success = false,
                message = "Invalid user token."
            });

        var result =
            await _addressService.DeleteAddressAsync(
                id,
                userId.Value);

        if (!result)
            return NotFound(new
            {
                success = false,
                message = "Address not found."
            });

        return Ok(new
        {
            success = true,
            message = "Address deleted successfully."
        });
    }

    private Guid? GetUserId()
    {
        var claim = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (Guid.TryParse(claim, out var userId))
            return userId;

        return null;
    }
}