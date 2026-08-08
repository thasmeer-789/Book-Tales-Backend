using BookTales.Application.DTOs.Wishlist;

namespace BookTales.Application.Interfaces.Services;

public interface IWishlistService
{
    Task<WishlistResponseDto> GetMyWishlistAsync(Guid userId);

    Task<bool> AddToWishlistAsync(
        Guid userId,
        AddWishlistItemDto request);

    Task<bool> RemoveFromWishlistAsync(
        Guid userId,
        Guid bookId);
}