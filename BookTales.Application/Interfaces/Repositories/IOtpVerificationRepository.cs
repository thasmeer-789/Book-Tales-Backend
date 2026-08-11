using BookTales.Domain.Entities;

namespace BookTales.Application.Interfaces.Repositories;

public interface IOtpVerificationRepository
{
    Task AddAsync(OtpVerification otp);

    Task<OtpVerification?> GetValidOtpAsync(
        Guid userId,
        string purpose,
        string codeHash);

    Task<OtpVerification?> GetLatestAsync(
        Guid userId,
        string purpose);

    void Update(OtpVerification otp);

    Task SaveChangesAsync();
}