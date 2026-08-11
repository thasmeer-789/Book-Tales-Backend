using BookTales.Application.Interfaces.Repositories;
using BookTales.Domain.Entities;
using BookTales.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookTales.Infrastructure.Repositories;

public class OtpVerificationRepository : IOtpVerificationRepository
{
    private readonly ApplicationDbContext _context;

    public OtpVerificationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(OtpVerification otp)
    {
        await _context.OtpVerifications.AddAsync(otp);
    }

    public async Task<OtpVerification?> GetValidOtpAsync(
        Guid userId,
        string purpose,
        string codeHash)
    {
        return await _context.OtpVerifications
            .FirstOrDefaultAsync(o =>
                o.UserId == userId &&
                o.Purpose == purpose &&
                o.CodeHash == codeHash &&
                !o.IsUsed &&
                o.ExpiresAt > DateTime.UtcNow);
    }

    public async Task<OtpVerification?> GetLatestAsync(
        Guid userId,
        string purpose)
    {
        return await _context.OtpVerifications
            .Where(o =>
                o.UserId == userId &&
                o.Purpose == purpose)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public void Update(OtpVerification otp)
    {
        _context.OtpVerifications.Update(otp);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}