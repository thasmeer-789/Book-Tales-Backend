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

            return services;
        }
    }
}