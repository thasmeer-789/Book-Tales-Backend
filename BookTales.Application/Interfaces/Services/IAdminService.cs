using BookTales.Application.DTOs.Admin;

namespace BookTales.Application.Interfaces.Services;

public interface IAdminService
{
    Task<DashboardStatisticsDto> GetDashboardStatisticsAsync();

    Task<SalesReportDto> GetSalesReportAsync();
    Task<List<RecentOrderDto>> GetRecentOrdersAsync();
    Task<List<TopSellingBookDto>> GetTopSellingBooksAsync();
}