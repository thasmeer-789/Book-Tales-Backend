using BookTales.Application.DTOs.Admin;
using BookTales.Application.DTOs.User;

namespace BookTales.Application.Interfaces.Services;

public interface IUserService
{
    Task<IEnumerable<AdminUserDto>> GetAllUsersAsync();

    Task<AdminUserDto?> GetUserByIdAsync(Guid id);

    Task<bool> BlockUserAsync(Guid id, Guid currentUserId);

    Task<bool> UnblockUserAsync(Guid id);

    Task<UserProfileDto?> GetMyProfileAsync(Guid userId);

    Task<UserProfileDto?> UpdateMyProfileAsync(Guid userId,UpdateUserProfileDto dto);

}