using BookTales.Application.DTOs.Auth;

namespace BookTales.Application.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(
        RegisterRequestDto request);

    Task<AuthResponseDto> LoginAsync(
        LoginRequestDto request);

    Task<bool> VerifyRegistrationOtpAsync(
        VerifyOtpRequestDto request);

    Task<bool> ResendRegistrationOtpAsync(
        string email);

    Task<bool> ForgotPasswordAsync(
        ForgotPasswordRequestDto request);

    Task<bool> VerifyPasswordResetOtpAsync(
        VerifyOtpRequestDto request);

    Task<bool> ResetPasswordAsync(
        ResetPasswordRequestDto request);
}