using AutoMapper;
using BookTales.Application.DTOs.Wishlist;
using BookTales.Application.Interfaces.Repositories;
using BookTales.Application.Interfaces.Services;
using BookTales.Domain.Entities;

namespace BookTales.Application.Services;

public class WishlistService : IWishlistService
{
    private readonly IWishlistRepository _wishlistRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;

    public WishlistService(
        IWishlistRepository wishlistRepository,
        IBookRepository bookRepository,
        IMapper mapper)
    {
        _wishlistRepository = wishlistRepository;
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task<WishlistResponseDto> GetMyWishlistAsync(Guid userId)
    {
        var wishlist = await _wishlistRepository.GetByUserIdAsync(userId);

        if (wishlist == null)
        {
            wishlist = new Wishlist
            {
                UserId = userId
            };

            await _wishlistRepository.AddWishlistAsync(wishlist);
            await _wishlistRepository.SaveChangesAsync();
        }

        return _mapper.Map<WishlistResponseDto>(wishlist);
    }

    public async Task<bool> AddToWishlistAsync(
        Guid userId,
        AddWishlistItemDto request)
    {
        var book = await _bookRepository.GetByIdAsync(request.BookId);

        if (book == null)
        {
            throw new InvalidOperationException("Book not found.");
        }

        var wishlist = await _wishlistRepository.GetByUserIdAsync(userId);

        if (wishlist == null)
        {
            wishlist = new Wishlist
            {
                UserId = userId
            };

            await _wishlistRepository.AddWishlistAsync(wishlist);
            await _wishlistRepository.SaveChangesAsync();
        }

        var existingItem = wishlist.WishlistItems
            .FirstOrDefault(item => item.BookId == request.BookId);

        if (existingItem != null)
        {
            return false;
        }

        var item = new WishlistItem
        {
            WishlistId = wishlist.Id,
            BookId = request.BookId
        };

        await _wishlistRepository.AddItemAsync(item);
        await _wishlistRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveFromWishlistAsync(
        Guid userId,
        Guid bookId)
    {
        var wishlist = await _wishlistRepository.GetByUserIdAsync(userId);

        if (wishlist == null)
        {
            return false;
        }

        var item = wishlist.WishlistItems
            .FirstOrDefault(item => item.BookId == bookId);

        if (item == null)
        {
            return false;
        }

        _wishlistRepository.RemoveItem(item);
        await _wishlistRepository.SaveChangesAsync();

        return true;
    }
}