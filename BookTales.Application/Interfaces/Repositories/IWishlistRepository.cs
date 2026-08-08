using BookTales.Domain.Entities;

namespace BookTales.Application.Interfaces.Repositories;

public interface IWishlistRepository
{
    Task<Wishlist?> GetByUserIdAsync(Guid userId);

    Task<WishlistItem?> GetItemAsync(Guid wishlistId, Guid bookId);

    Task AddWishlistAsync(Wishlist wishlist);

    Task AddItemAsync(WishlistItem item);

    void RemoveItem(WishlistItem item);

    Task SaveChangesAsync();
}