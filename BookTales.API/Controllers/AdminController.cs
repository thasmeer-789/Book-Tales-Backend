using BookTales.Application.DTOs.Admin;
using BookTales.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookTales.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("dashboard/statistics")]
    public async Task<ActionResult<DashboardStatisticsDto>>
        GetDashboardStatistics()
    {
        var result =
            await _adminService.GetDashboardStatisticsAsync();

        return Ok(result);
    }

    [HttpGet("dashboard/sales-report")]
    public async Task<ActionResult<SalesReportDto>>
    GetSalesReport()
    {
        var result =
            await _adminService.GetSalesReportAsync();

        return Ok(result);
    }

    [HttpGet("dashboard/recent-orders")]
    public async Task<ActionResult<List<RecentOrderDto>>>
    GetRecentOrders()
    {
        var result =
            await _adminService.GetRecentOrdersAsync();

        return Ok(result);
    }

    [HttpGet("dashboard/top-selling-books")]
    public async Task<ActionResult<List<TopSellingBookDto>>>
    GetTopSellingBooks()
    {
        var result =
            await _adminService.GetTopSellingBooksAsync();

        return Ok(result);
    }
}