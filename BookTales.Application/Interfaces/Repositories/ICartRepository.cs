using BookTales.Domain.Entities;

namespace BookTales.Application.Interfaces.Repositories;

public interface ICartRepository
{
    Task<Cart?> GetByUserIdAsync(Guid userId);

    Task<CartItem?> GetItemAsync(
        Guid cartId,
        Guid bookId);

    Task AddCartAsync(Cart cart);

    Task AddItemAsync(CartItem item);

    void UpdateItem(CartItem item);

    void RemoveItem(CartItem item);

    Task SaveChangesAsync();
}