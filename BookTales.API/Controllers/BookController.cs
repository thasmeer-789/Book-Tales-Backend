using BookTales.Application.DTOs.Book;
using BookTales.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookTales.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _bookService.GetAllAsync();

        return Ok(response);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string search)
    {
        var response = await _bookService.SearchAsync(search);

        return Ok(response);
    }

    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> GetByCategory(Guid categoryId)
    {
        var response = await _bookService.GetByCategoryAsync(categoryId);

        return Ok(response);
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
    {
        var result = await _bookService.GetPagedAsync(
            pageNumber,
            pageSize);

        var totalPages = (int)Math.Ceiling(
            result.TotalCount / (double)pageSize);

        return Ok(new
        {
            books = result.Books,
            pageNumber,
            pageSize,
            totalCount = result.TotalCount,
            totalPages
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await _bookService.GetByIdAsync(id);

        if (response == null)
            return NotFound();

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBookDto request)
    {
        var response = await _bookService.CreateAsync(request);

        return Ok(response);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateBookDto request)
    {
        var result = await _bookService.UpdateAsync(id, request);

        if (!result)
            return NotFound();

        return Ok(new
        {
            success = true,
            message = "Book updated successfully."
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _bookService.DeleteAsync(id);

        if (!result)
            return NotFound();

        return Ok(new
        {
            success = true,
            message = "Book deleted successfully."
        });
    }
}