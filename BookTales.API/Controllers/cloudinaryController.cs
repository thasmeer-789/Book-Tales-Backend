using BookTales.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookTales.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class CloudinaryTestController : ControllerBase
{
    private readonly ICloudinaryService _cloudinaryService;

    public CloudinaryTestController(ICloudinaryService cloudinaryService)
    {
        _cloudinaryService = cloudinaryService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new
            {
                success = false,
                message = "Please select an image."
            });
        }

        try
        {
            var imageUrl = await _cloudinaryService.UploadImageAsync(
                file.OpenReadStream(),
                file.FileName);

            return Ok(new
            {
                success = true,
                message = "Image uploaded successfully.",
                imageUrl
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
    }
}