using BookTales.Application.DTOs.Book;

namespace BookTales.Application.Interfaces.Services;

public interface IBookService
{
    Task<IEnumerable<BookResponseDto>> GetAllAsync();

    Task<BookResponseDto?> GetByIdAsync(Guid id);

    Task<IEnumerable<BookResponseDto>> SearchAsync(string search);

    Task<IEnumerable<BookResponseDto>> GetByCategoryAsync(Guid categoryId);
    Task<(IEnumerable<BookResponseDto> Books, int TotalCount)> GetPagedAsync(
    int pageNumber,
    int pageSize);

    Task<BookResponseDto> CreateAsync(CreateBookDto request);

    Task<bool> UpdateAsync(Guid id, UpdateBookDto request);

    Task<bool> DeleteAsync(Guid id);
}