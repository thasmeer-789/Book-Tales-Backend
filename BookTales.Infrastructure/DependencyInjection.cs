using BookTales.Application.Interfaces.Repositories;
using BookTales.Application.Interfaces.Services;
using BookTales.Application.Services;
using BookTales.Infrastructure.Identity;
using BookTales.Infrastructure.Persistence;
using BookTales.Infrastructure.Repositories;
using BookTales.Infrastructure.Services;
using BookTales.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace BookTales.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        configuration.GetConnectionString("DefaultConnection"),
        b =>
        {
            b.MigrationsAssembly(
                typeof(ApplicationDbContext).Assembly.FullName);

            b.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null);
        }));

        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<EmailSettings>(
             configuration.GetSection("EmailSettings"));

        services.Configure<CloudinarySettings>(
             configuration.GetSection("CloudinarySettings"));

        services.Configure<RazorpaySettings>(
            configuration.GetSection("Razorpay"));      

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IOtpVerificationRepository, OtpVerificationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IWishlistRepository, WishlistRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<ICloudinaryService, CloudinaryService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IAddressService, AddressService>();

        return services;
    }
}