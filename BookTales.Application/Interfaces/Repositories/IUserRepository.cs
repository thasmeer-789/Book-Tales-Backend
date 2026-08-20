using BookTales.Domain.Entities;

namespace BookTales.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);

    Task<IEnumerable<User>> GetAllAsync();

    Task<User?> GetByIdAsync(Guid id);

    Task<User?> GetByIdForUpdateAsync(Guid id);

    Task<bool> IsAdminAsync(Guid domainUserId);

    Task AddAsync(User user);

    Task SaveChangesAsync();
}