using BookTales.Application.DTOs.Admin;

namespace BookTales.Application.Interfaces.Services;

public interface IUserService
{
    Task<IEnumerable<AdminUserDto>> GetAllUsersAsync();

    Task<AdminUserDto?> GetUserByIdAsync(Guid id);

    Task<bool> BlockUserAsync(Guid id, Guid currentUserId);

    Task<bool> UnblockUserAsync(Guid id);
}