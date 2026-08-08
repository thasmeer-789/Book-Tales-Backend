using AutoMapper;
using BookTales.Application.DTOs.Cart;
using BookTales.Domain.Entities;

namespace BookTales.Application.Mappings;

public class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<Cart, CartResponseDto>()
            .ForMember(
                dest => dest.Items,
                opt => opt.MapFrom(src => src.CartItems));

        CreateMap<CartItem, CartItemResponseDto>()
            .ForMember(
                dest => dest.BookTitle,
                opt => opt.MapFrom(src =>
                    src.Book != null ? src.Book.Title : string.Empty))
            .ForMember(
                dest => dest.Price,
                opt => opt.MapFrom(src =>
                    src.Book != null ? src.Book.Price : 0))
            .ForMember(
                dest => dest.ImageUrl,
                opt => opt.MapFrom(src =>
                    src.Book != null ? src.Book.ImageUrl : string.Empty))
            .ForMember(
                dest => dest.Subtotal,
                opt => opt.MapFrom(src =>
                    src.Book != null
                        ? src.Book.Price * src.Quantity
                        : 0));
    }
}