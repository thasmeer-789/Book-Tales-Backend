using BookTales.Domain.Entities;

namespace BookTales.Application.Interfaces.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();

    Task<Book?> GetByIdAsync(Guid id);

    Task<IEnumerable<Book>> SearchAsync(string search);

    Task<IEnumerable<Book>> GetByCategoryAsync(Guid categoryId);
    Task<(IEnumerable<Book> Books, int TotalCount)> GetPagedAsync(
    int pageNumber,
    int pageSize);

    Task AddAsync(Book book);

    void Update(Book book);

    void Delete(Book book);

    Task SaveChangesAsync();
}