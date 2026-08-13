using AutoMapper;
using BookTales.Application.DTOs.Cart;
using BookTales.Application.Interfaces.Repositories;
using BookTales.Application.Interfaces.Services;
using BookTales.Domain.Entities;

namespace BookTales.Application.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;

    public CartService(
        ICartRepository cartRepository,
        IBookRepository bookRepository,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task<CartResponseDto> GetMyCartAsync(Guid userId)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId);

        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId
            };

            await _cartRepository.AddCartAsync(cart);
            await _cartRepository.SaveChangesAsync();
        }

        var response = _mapper.Map<CartResponseDto>(cart);

        response.Total = response.Items.Sum(item => item.Subtotal);

        return response;
    }

    public async Task<bool> AddToCartAsync(
        Guid userId,
        AddCartItemDto request)
    {
        if (request.Quantity <= 0)
            return false;

        var book = await _bookRepository.GetByIdAsync(request.BookId);

        if (book == null)
            return false;

        var cart = await _cartRepository.GetByUserIdAsync(userId);

        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId
            };

            await _cartRepository.AddCartAsync(cart);
            await _cartRepository.SaveChangesAsync();
        }

        var existingItem = cart.CartItems
            .FirstOrDefault(item => item.BookId == request.BookId);

        if (existingItem != null)
        {
            var newQuantity = existingItem.Quantity + request.Quantity;

            if (newQuantity > book.Stock)
                return false;

            existingItem.Quantity = newQuantity;
        }
        else
        {
            if (request.Quantity > book.Stock)
                return false;

            var item = new CartItem
            {
                CartId = cart.Id,
                BookId = book.Id,
                Quantity = request.Quantity
            };

            await _cartRepository.AddItemAsync(item);
        }

        await _cartRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateCartItemAsync(
        Guid userId,
        Guid bookId,
        UpdateCartItemDto request)
    {
        if (request.Quantity <= 0)
            return false;

        var cart = await _cartRepository.GetByUserIdAsync(userId);

        if (cart == null)
            return false;

        var item = cart.CartItems
            .FirstOrDefault(item => item.BookId == bookId);

        if (item == null)
            return false;

        var book = await _bookRepository.GetByIdAsync(bookId);

        if (book == null)
            return false;

        if (request.Quantity > book.Stock)
            return false;

        item.Quantity = request.Quantity;

        await _cartRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveFromCartAsync(
        Guid userId,
        Guid bookId)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId);

        if (cart == null)
            return false;

        var item = cart.CartItems
            .FirstOrDefault(item => item.BookId == bookId);

        if (item == null)
            return false;

        _cartRepository.RemoveItem(item);

        await _cartRepository.SaveChangesAsync();

        return true;
    }
}