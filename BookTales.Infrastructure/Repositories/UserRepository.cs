using BookTales.Application.Interfaces.Repositories;
using BookTales.Domain.Entities;
using BookTales.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookTales.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
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
}