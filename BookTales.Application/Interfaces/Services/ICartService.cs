using BookTales.Application.DTOs.Cart;

namespace BookTales.Application.Interfaces.Services;

public interface ICartService
{
    Task<CartResponseDto> GetMyCartAsync(Guid userId);

    Task<bool> AddToCartAsync(
        Guid userId,
        AddCartItemDto request);

    Task<bool> UpdateCartItemAsync(
        Guid userId,
        Guid bookId,
        UpdateCartItemDto request);

    Task<bool> RemoveFromCartAsync(
        Guid userId,
        Guid bookId);
}