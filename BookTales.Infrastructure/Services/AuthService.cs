using BookTales.Application.Constants;
using BookTales.Application.DTOs.Auth;
using BookTales.Application.Interfaces.Repositories;
using BookTales.Application.Interfaces.Services;
using BookTales.Domain.Entities;
using BookTales.Infrastructure.Identity;
using BookTales.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace BookTales.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IEmailService _emailService;
    private readonly ApplicationDbContext _context;
    private readonly IOtpVerificationRepository _otpRepository;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        IUserRepository userRepository,
        IJwtService jwtService,
        IEmailService emailService,
        IOtpVerificationRepository otpRepository,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _jwtService = jwtService;
        _emailService = emailService;
        _otpRepository = otpRepository;
        _context = context;
    }

    public async Task<AuthResponseDto> RegisterAsync(
        RegisterRequestDto request)
    {
        if (request.Password != request.ConfirmPassword)
            throw new Exception("Passwords do not match.");

        var existingIdentityUser =
            await _userManager.FindByEmailAsync(request.Email);

        if (existingIdentityUser != null)
            throw new Exception("Email already exists.");

        var existingDomainUser =
            await _userRepository.GetByEmailAsync(request.Email);

        if (existingDomainUser != null)
            throw new Exception("Email already exists.");

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            // 1. Create Domain User
            var domainUser = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            await _userRepository.AddAsync(domainUser);
            await _userRepository.SaveChangesAsync();

            // 2. Create Identity User
            var applicationUser = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                DomainUserId = domainUser.Id
            };

            var result = await _userManager.CreateAsync(
                applicationUser,
                request.Password);

            if (!result.Succeeded)
            {
                throw new Exception(
                    string.Join(
                        ", ",
                        result.Errors.Select(e => e.Description)));
            }

            // 3. Add User Role
            var roleResult =
                await _userManager.AddToRoleAsync(
                    applicationUser,
                    "User");

            if (!roleResult.Succeeded)
            {
                throw new Exception(
                    string.Join(
                        ", ",
                        roleResult.Errors.Select(e => e.Description)));
            }

            // 4. Invalidate previous registration OTPs
            await _otpRepository.InvalidatePreviousOtpsAsync(
                domainUser.Id,
                OtpPurpose.RegisterVerification);

            // 5. Generate OTP
            var otp = GenerateOtp();

            var otpHash = Convert.ToBase64String(
                SHA256.HashData(
                    Encoding.UTF8.GetBytes(otp)));

            // 6. Store OTP
            var otpVerification = new OtpVerification
            {
                UserId = domainUser.Id,
                CodeHash = otpHash,
                Purpose = OtpPurpose.RegisterVerification,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false
            };

            await _otpRepository.AddAsync(otpVerification);
            await _otpRepository.SaveChangesAsync();

            // 7. Send OTP Email
            await _emailService.SendEmailAsync(
                applicationUser.Email!,
                "Book-Tales Registration OTP",
                $"Your Book-Tales verification OTP is: {otp}. " +
                "It expires in 5 minutes.");

            // 8. Commit transaction
            await transaction.CommitAsync();

            return new AuthResponseDto
            {
                Success = true,
                Message =
                    "Registration successful. Please verify your email using the OTP sent to you.",
                Email = domainUser.Email,
                FirstName = domainUser.FirstName,
                LastName = domainUser.LastName,
                Roles = new List<string> { "User" }
            };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<AuthResponseDto> LoginAsync(
        LoginRequestDto request)
    {
        var user =
            await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            throw new Exception(
                "Invalid email or password.");
        }

        var isPasswordValid =
            await _userManager.CheckPasswordAsync(
                user,
                request.Password);

        if (!isPasswordValid)
        {
            throw new Exception(
                "Invalid email or password.");
        }

        var roles =
            await _userManager.GetRolesAsync(user);

        var token = _jwtService.GenerateToken(
            user.DomainUserId,
            user.Email!,
            roles);

        return new AuthResponseDto
        {
            Success = true,
            Message = "Login successful.",
            Token = token,
            Email = user.Email!,
            Roles = roles
        };
    }

    public async Task<bool> VerifyRegistrationOtpAsync(
        VerifyOtpRequestDto request)
    {
        var user =
            await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
            return false;

        var otpHash = Convert.ToBase64String(
            SHA256.HashData(
                Encoding.UTF8.GetBytes(request.Otp)));

        var otp =
            await _otpRepository.GetValidOtpAsync(
                user.DomainUserId,
                OtpPurpose.RegisterVerification,
                otpHash);

        if (otp == null)
            return false;

        otp.IsUsed = true;

        _otpRepository.Update(otp);

        await _otpRepository.SaveChangesAsync();

        user.EmailConfirmed = true;

        await _userManager.UpdateAsync(user);

        return true;
    }

    public async Task<bool> ResendRegistrationOtpAsync(
        string email)
    {
        var user =
            await _userManager.FindByEmailAsync(email);

        if (user == null)
            return false;

        if (user.EmailConfirmed)
            return false;

        // Invalidate previous registration OTPs
        await _otpRepository.InvalidatePreviousOtpsAsync(
            user.DomainUserId,
            OtpPurpose.RegisterVerification);

        // Generate new OTP
        var otp = GenerateOtp();

        var otpHash = Convert.ToBase64String(
            SHA256.HashData(
                Encoding.UTF8.GetBytes(otp)));

        var otpVerification = new OtpVerification
        {
            UserId = user.DomainUserId,
            CodeHash = otpHash,
            Purpose = OtpPurpose.RegisterVerification,
            ExpiresAt = DateTime.UtcNow.AddMinutes(5),
            IsUsed = false
        };

        await _otpRepository.AddAsync(
            otpVerification);

        await _otpRepository.SaveChangesAsync();

        await _emailService.SendEmailAsync(
            user.Email!,
            "Book-Tales Registration OTP",
            $"Your Book-Tales verification OTP is: {otp}. " +
            "It expires in 5 minutes.");

        return true;
    }

    public async Task<bool> ForgotPasswordAsync(
        ForgotPasswordRequestDto request)
    {
        var user =
            await _userManager.FindByEmailAsync(
                request.Email);

        if (user == null)
            return false;

        // Invalidate previous password reset OTPs
        await _otpRepository.InvalidatePreviousOtpsAsync(
            user.DomainUserId,
            OtpPurpose.PasswordReset);

        // Generate new OTP
        var otp = GenerateOtp();

        var otpHash = Convert.ToBase64String(
            SHA256.HashData(
                Encoding.UTF8.GetBytes(otp)));

        var otpVerification = new OtpVerification
        {
            UserId = user.DomainUserId,
            CodeHash = otpHash,
            Purpose = OtpPurpose.PasswordReset,
            ExpiresAt = DateTime.UtcNow.AddMinutes(5),
            IsUsed = false
        };

        await _otpRepository.AddAsync(
            otpVerification);

        await _otpRepository.SaveChangesAsync();

        await _emailService.SendEmailAsync(
            user.Email!,
            "Book-Tales Password Reset OTP",
            $"Your Book-Tales password reset OTP is: {otp}. " +
            "It expires in 5 minutes.");

        return true;
    }

    public async Task<bool> VerifyPasswordResetOtpAsync(
        VerifyOtpRequestDto request)
    {
        var user =
            await _userManager.FindByEmailAsync(
                request.Email);

        if (user == null)
            return false;

        var otpHash = Convert.ToBase64String(
            SHA256.HashData(
                Encoding.UTF8.GetBytes(request.Otp)));

        var otp =
            await _otpRepository.GetValidOtpAsync(
                user.DomainUserId,
                OtpPurpose.PasswordReset,
                otpHash);

        if (otp == null)
            return false;

        return true;
    }

    public async Task<bool> ResetPasswordAsync(
        ResetPasswordRequestDto request)
    {
        if (request.NewPassword != request.ConfirmPassword)
            return false;

        var user =
            await _userManager.FindByEmailAsync(
                request.Email);

        if (user == null)
            return false;

        var otpHash = Convert.ToBase64String(
            SHA256.HashData(
                Encoding.UTF8.GetBytes(request.Otp)));

        var otp =
            await _otpRepository.GetValidOtpAsync(
                user.DomainUserId,
                OtpPurpose.PasswordReset,
                otpHash);

        if (otp == null)
            return false;

        var token =
            await _userManager.GeneratePasswordResetTokenAsync(
                user);

        var result =
            await _userManager.ResetPasswordAsync(
                user,
                token,
                request.NewPassword);

        if (!result.Succeeded)
            return false;

        otp.IsUsed = true;

        _otpRepository.Update(otp);

        await _otpRepository.SaveChangesAsync();

        return true;
    }

    private string GenerateOtp()
    {
        return RandomNumberGenerator
            .GetInt32(100000, 1000000)
            .ToString();
    }
}