using BookTales.Application.DTOs.Admin;
using BookTales.Application.DTOs.User;
using BookTales.Application.Interfaces.Repositories;
using BookTales.Application.Interfaces.Services;

namespace BookTales.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<AdminUserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users.Select(user => new AdminUserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            IsBlocked = user.IsBlocked
        });
    }

    public async Task<AdminUserDto?> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
            return null;

        return new AdminUserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            IsBlocked = user.IsBlocked
        };
    }

    public async Task<bool> BlockUserAsync(
     Guid id,
     Guid currentUserId)
    {
        if (id == currentUserId)
            throw new InvalidOperationException(
                "You cannot block your own account.");

        var user = await _userRepository.GetByIdForUpdateAsync(id);

        if (user == null)
            return false;

        var isAdmin = await _userRepository.IsAdminAsync(id);

        if (isAdmin)
            throw new InvalidOperationException(
                "Admin accounts cannot be blocked.");

        user.IsBlocked = true;

        await _userRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UnblockUserAsync(Guid id)
    {
        var user = await _userRepository.GetByIdForUpdateAsync(id);

        if (user == null)
            return false;

        user.IsBlocked = false;

        await _userRepository.SaveChangesAsync();

        return true;
    }

    public async Task<UserProfileDto?> GetMyProfileAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
            return null;

        return new UserProfileDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
    }

    public async Task<UserProfileDto?> UpdateMyProfileAsync(
        Guid userId,
        UpdateUserProfileDto dto)
    {
        var user = await _userRepository.GetByIdForUpdateAsync(userId);

        if (user == null)
            return null;

        user.FirstName = dto.FirstName.Trim();
        user.LastName = dto.LastName.Trim();
        user.PhoneNumber = dto.PhoneNumber.Trim();

        await _userRepository.SaveChangesAsync();

        return new UserProfileDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
    }
}