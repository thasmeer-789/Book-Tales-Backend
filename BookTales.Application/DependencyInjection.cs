using BookTales.Application.Interfaces.Repositories;
using BookTales.Application.Interfaces.Services;
using BookTales.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BookTales.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { }, typeof(CategoryService).Assembly);
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IWishlistService, WishlistService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}