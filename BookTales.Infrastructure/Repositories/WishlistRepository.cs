using BookTales.Application.Interfaces.Repositories;
using BookTales.Domain.Entities;
using BookTales.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookTales.Infrastructure.Repositories;

public class WishlistRepository : IWishlistRepository
{
    private readonly ApplicationDbContext _context;

    public WishlistRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Wishlist?> GetByUserIdAsync(Guid userId)
    {
        return await _context.Wishlists
            .Include(w => w.WishlistItems)
            .ThenInclude(wi => wi.Book)
            .FirstOrDefaultAsync(w => w.UserId == userId);
    }

    public async Task<WishlistItem?> GetItemAsync(
        Guid wishlistId,
        Guid bookId)
    {
        return await _context.WishlistItems
            .FirstOrDefaultAsync(wi =>
                wi.WishlistId == wishlistId &&
                wi.BookId == bookId);
    }

    public async Task AddWishlistAsync(Wishlist wishlist)
    {
        await _context.Wishlists.AddAsync(wishlist);
    }

    public async Task AddItemAsync(WishlistItem item)
    {
        await _context.WishlistItems.AddAsync(item);
    }

    public void RemoveItem(WishlistItem item)
    {
        _context.WishlistItems.Remove(item);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}