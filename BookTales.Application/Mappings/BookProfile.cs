using AutoMapper;
using BookTales.Application.DTOs.Book;
using BookTales.Domain.Entities;

namespace BookTales.Application.Mappings;

public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<CreateBookDto, Book>();

        CreateMap<UpdateBookDto, Book>();

        CreateMap<Book, BookResponseDto>()
            .ForMember(
                dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category != null
                    ? src.Category.Name
                    : null));
    }
}