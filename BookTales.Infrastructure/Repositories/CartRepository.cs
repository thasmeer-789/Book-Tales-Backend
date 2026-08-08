using BookTales.Application.Interfaces.Repositories;
using BookTales.Domain.Entities;
using BookTales.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookTales.Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private readonly ApplicationDbContext _context;

    public CartRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Cart?> GetByUserIdAsync(Guid userId)
    {
        return await _context.Carts
            .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Book)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task<CartItem?> GetItemAsync(
        Guid cartId,
        Guid bookId)
    {
        return await _context.CartItems
            .FirstOrDefaultAsync(ci =>
                ci.CartId == cartId &&
                ci.BookId == bookId);
    }

    public async Task AddCartAsync(Cart cart)
    {
        await _context.Carts.AddAsync(cart);
    }

    public async Task AddItemAsync(CartItem item)
    {
        await _context.CartItems.AddAsync(item);
    }

    public void UpdateItem(CartItem item)
    {
        _context.CartItems.Update(item);
    }

    public void RemoveItem(CartItem item)
    {
        _context.CartItems.Remove(item);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}