using AutoMapper;
using BookTales.Application.DTOs.Category;
using BookTales.Domain.Entities;

namespace BookTales.Application.Mappings;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CreateCategoryDto, Category>();

        CreateMap<UpdateCategoryDto, Category>();

        CreateMap<Category, CategoryResponseDto>();
    }
}