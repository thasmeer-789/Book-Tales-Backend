using BookTales.Application.DTOs.Auth;
using BookTales.Application.Interfaces.Repositories;
using BookTales.Application.Interfaces.Services;
using BookTales.Domain.Entities;
using BookTales.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace BookTales.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public AuthService(
     UserManager<ApplicationUser> userManager,
     IUserRepository userRepository,
     IJwtService jwtService)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        if (request.Password != request.ConfirmPassword)
            throw new Exception("Passwords do not match.");

        var existingIdentityUser = await _userManager.FindByEmailAsync(request.Email);

        if (existingIdentityUser != null)
            throw new Exception("Email already exists.");

        var existingDomainUser = await _userRepository.GetByEmailAsync(request.Email);

        if (existingDomainUser != null)
            throw new Exception("Email already exists.");

        var domainUser = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };

        await _userRepository.AddAsync(domainUser);
        await _userRepository.SaveChangesAsync();

        var applicationUser = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            DomainUserId = domainUser.Id
        };

        var result = await _userManager.CreateAsync(applicationUser, request.Password);

        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        await _userManager.AddToRoleAsync(applicationUser, "User");

        return new AuthResponseDto
        {
            Success = true,
            Message = "User registered successfully.",
            Email = domainUser.Email,
            FirstName = domainUser.FirstName,
            LastName = domainUser.LastName,
            Roles = new List<string> { "User" }
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            throw new Exception("Invalid email or password.");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!isPasswordValid)
        {
            throw new Exception("Invalid email or password.");
        }

        var roles = await _userManager.GetRolesAsync(user);

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
}