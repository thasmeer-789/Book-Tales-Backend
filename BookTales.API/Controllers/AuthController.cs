using BookTales.Application.DTOs.Auth;
using BookTales.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookTales.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterRequestDto request)
    {
        var response = await _authService.RegisterAsync(request);

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginRequestDto request)
    {
        var response = await _authService.LoginAsync(request);

        return Ok(response);
    }

    [HttpPost("verify-registration-otp")]
    public async Task<IActionResult> VerifyRegistrationOtp(
        VerifyOtpRequestDto request)
    {
        var result =
            await _authService.VerifyRegistrationOtpAsync(request);

        if (!result)
        {
            return BadRequest(new
            {
                success = false,
                message = "Invalid or expired OTP."
            });
        }

        return Ok(new
        {
            success = true,
            message = "Email verified successfully."
        });
    }

    [HttpPost("resend-registration-otp")]
    public async Task<IActionResult> ResendRegistrationOtp(
        [FromQuery] string email)
    {
        var result =
            await _authService.ResendRegistrationOtpAsync(email);

        if (!result)
        {
            return BadRequest(new
            {
                success = false,
                message = "Unable to resend OTP."
            });
        }

        return Ok(new
        {
            success = true,
            message = "A new OTP has been sent."
        });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(
    ForgotPasswordRequestDto request)
    {
        var result = await _authService.ForgotPasswordAsync(request);

        if (!result)
        {
            return BadRequest(new
            {
                success = false,
                message = "Unable to process password reset request."
            });
        }

        return Ok(new
        {
            success = true,
            message = "Password reset OTP has been sent to your email."
        });
    }

    [HttpPost("verify-password-reset-otp")]
    public async Task<IActionResult> VerifyPasswordResetOtp(
    VerifyOtpRequestDto request)
    {
        var result =
            await _authService.VerifyPasswordResetOtpAsync(request);

        if (!result)
        {
            return BadRequest(new
            {
                success = false,
                message = "Invalid or expired OTP."
            });
        }

        return Ok(new
        {
            success = true,
            message = "Password reset OTP is valid."
        });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(
    ResetPasswordRequestDto request)
    {
        var result =
            await _authService.ResetPasswordAsync(request);

        if (!result)
        {
            return BadRequest(new
            {
                success = false,
                message = "Unable to reset password."
            });
        }

        return Ok(new
        {
            success = true,
            message = "Password reset successfully."
        });
    }

}