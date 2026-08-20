using BookTales.Application.DTOs.Admin;
using BookTales.Application.Interfaces.Services;
using BookTales.Domain.Enums;
using BookTales.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookTales.Infrastructure.Services;

public class AdminService : IAdminService
{
    private readonly ApplicationDbContext _context;
    public AdminService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardStatisticsDto> GetDashboardStatisticsAsync()
    {
        var totalUsers = await _context.DomainUsers.CountAsync();

        var totalBooks = await _context.Books.CountAsync();

        var totalOrders = await _context.Orders.CountAsync();

        var totalRevenue = await _context.Orders
      .Where(o => o.PaymentStatus == PaymentStatus.Paid)
      .SumAsync(o => o.TotalAmount);


        return new DashboardStatisticsDto
        {
            TotalUsers = totalUsers,
            TotalBooks = totalBooks,
            TotalOrders = totalOrders,
            TotalRevenue = totalRevenue
        };
    }

    public async Task<SalesReportDto> GetSalesReportAsync()
    {
        var paidOrders = await _context.Orders
        .Where(o => o.PaymentStatus == PaymentStatus.Paid)
         .ToListAsync();

        var totalRevenue = paidOrders.Sum(o => o.TotalAmount);

        var totalOrders = await _context.Orders.CountAsync();

        var paidOrderCount = paidOrders.Count;

        var averageOrderValue = paidOrderCount > 0
            ? totalRevenue / paidOrderCount
            : 0;

        return new SalesReportDto
        {
            TotalRevenue = totalRevenue,
            TotalOrders = totalOrders,
            PaidOrders = paidOrderCount,
            AverageOrderValue = averageOrderValue
        };
    }

    public async Task<List<RecentOrderDto>> GetRecentOrdersAsync()
    {
        var orders = await _context.Orders
            .Include(o => o.User)
            .OrderByDescending(o => o.OrderDate)
            .Take(5)
            .ToListAsync();

        return orders.Select(o => new RecentOrderDto
        {
            OrderId = o.Id,
            UserId = o.UserId,
            CustomerName = o.User != null
                ? $"{o.User.FirstName} {o.User.LastName}"
                : "Unknown",
            OrderDate = o.OrderDate,
            TotalAmount = o.TotalAmount,
            Status = o.Status.ToString(),
            PaymentStatus = o.PaymentStatus.ToString()
        }).ToList(); 
    }

    public async Task<List<TopSellingBookDto>> GetTopSellingBooksAsync()
    {
        var topBooks = await _context.OrderItems
            .Include(oi => oi.Book)
            .Where(oi => oi.Order != null && oi.Order.Status != OrderStatus.Cancelled)
            .GroupBy(oi => new
            {
                oi.BookId,
                BookTitle = oi.Book != null
                    ? oi.Book.Title
                    : "Unknown"
            })
            .Select(g => new TopSellingBookDto
            {
                BookId = g.Key.BookId,
                BookTitle = g.Key.BookTitle,
                TotalQuantitySold = g.Sum(oi => oi.Quantity),
                TotalSales = g.Sum(oi => oi.Quantity * oi.Price)
            })
            .OrderByDescending(x => x.TotalQuantitySold)
            .Take(5)
            .ToListAsync();

        return topBooks;
    }
}