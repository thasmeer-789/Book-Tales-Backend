using AutoMapper;
using BookTales.Application.DTOs.Wishlist;
using BookTales.Application.Interfaces.Repositories;
using BookTales.Application.Interfaces.Services;
using BookTales.Domain.Entities;

namespace BookTales.Application.Services;

public class WishlistService : IWishlistService
{
    private readonly IWishlistRepository _wishlistRepository;
    private readonly IMapper _mapper;

    public WishlistService(
        IWishlistRepository wishlistRepository,
        IMapper mapper)
    {
        _wishlistRepository = wishlistRepository;
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

        var existingItem = await _wishlistRepository.GetItemAsync(
            wishlist.Id,
            request.BookId);

        if (existingItem != null)
            return false;

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
            return false;

        var item = await _wishlistRepository.GetItemAsync(
            wishlist.Id,
            bookId);

        if (item == null)
            return false;

        _wishlistRepository.RemoveItem(item);
        await _wishlistRepository.SaveChangesAsync();

        return true;
    }
}