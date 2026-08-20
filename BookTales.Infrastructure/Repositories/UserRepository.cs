using BookTales.Application.Interfaces.Repositories;
using BookTales.Domain.Entities;
using BookTales.Infrastructure.Identity;
using BookTales.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookTales.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(
     ApplicationDbContext context,
     UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<bool> IsAdminAsync(Guid domainUserId)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.DomainUserId == domainUserId);

        if (user == null)
            return false;

        return await _userManager.IsInRoleAsync(user, "Admin");
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.DomainUsers
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task AddAsync(User user)
    {
        await _context.DomainUsers.AddAsync(user);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.DomainUsers
            .AsNoTracking()
            .OrderBy(u => u.FirstName)
            .ThenBy(u => u.LastName)
            .ToListAsync();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.DomainUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByIdForUpdateAsync(Guid id)
    {
        return await _context.DomainUsers
            .FirstOrDefaultAsync(u => u.Id == id);
    }
}