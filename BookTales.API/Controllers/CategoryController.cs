using BookTales.Application.DTOs.Category;
using BookTales.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookTales.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _categoryService.GetAllAsync();

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await _categoryService.GetByIdAsync(id);

        if (response == null)
            return NotFound();

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryDto request)
    {
        var response = await _categoryService.CreateAsync(request);

        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateCategoryDto request)
    {
        var result = await _categoryService.UpdateAsync(id, request);

        if (!result)
            return NotFound();

        return Ok(new
        {
            success = true,
            message = "Category updated successfully."
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _categoryService.DeleteAsync(id);

        if (!result)
            return NotFound();

        return Ok(new
        {
            success = true,
            message = "Category deleted successfully."
        });
    }
}