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
            .AsNoTracking()
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
            .AsNoTracking()
            .Where(o =>
                o.UserId == userId &&
                o.Purpose == purpose)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task InvalidatePreviousOtpsAsync(
        Guid userId,
        string purpose)
    {
        await _context.OtpVerifications
            .Where(o =>
                o.UserId == userId &&
                o.Purpose == purpose &&
                !o.IsUsed)
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(
                    o => o.IsUsed,
                    true));
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