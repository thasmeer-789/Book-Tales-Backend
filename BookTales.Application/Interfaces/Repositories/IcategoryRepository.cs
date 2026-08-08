using BookTales.Domain.Entities;

namespace BookTales.Application.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();

    Task<Category?> GetByIdAsync(Guid id);

    Task AddAsync(Category category);

    void Update(Category category);

    void Delete(Category category);

    Task SaveChangesAsync();
}