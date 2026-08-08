using BookTales.Application.DTOs.Category;

namespace BookTales.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponseDto>> GetAllAsync();

    Task<CategoryResponseDto?> GetByIdAsync(Guid id);

    Task<CategoryResponseDto> CreateAsync(CreateCategoryDto request);

    Task<bool> UpdateAsync(Guid id, UpdateCategoryDto request);

    Task<bool> DeleteAsync(Guid id);
}