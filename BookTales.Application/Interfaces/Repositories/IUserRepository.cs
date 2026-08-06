using BookTales.Infrastructure.Identity;

namespace BookTales.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<ApplicationUser?> GetByEmailAsync(string email);

    Task<ApplicationUser?> GetByIdAsync(string id);

    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);

    Task CreateAsync(ApplicationUser user, string password);

    Task AddToRoleAsync(ApplicationUser user, string role);

    Task<IList<string>> GetRolesAsync(ApplicationUser user);
}