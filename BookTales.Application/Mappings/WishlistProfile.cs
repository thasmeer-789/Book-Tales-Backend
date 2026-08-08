using AutoMapper;
using BookTales.Application.DTOs.Wishlist;
using BookTales.Domain.Entities;

namespace BookTales.Application.Mappings;

public class WishlistProfile : Profile
{
    public WishlistProfile()
    {
        CreateMap<Wishlist, WishlistResponseDto>()
            .ForMember(
                dest => dest.Items,
                opt => opt.MapFrom(src => src.WishlistItems));

        CreateMap<WishlistItem, WishlistItemResponseDto>()
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
                    src.Book != null ? src.Book.ImageUrl : string.Empty));
    }
}