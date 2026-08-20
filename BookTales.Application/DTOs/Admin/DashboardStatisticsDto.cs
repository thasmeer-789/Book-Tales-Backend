namespace BookTales.Application.DTOs.Admin;

public class DashboardStatisticsDto
{
    public int TotalUsers { get; set; }
    public int TotalBooks { get; set; }
    public int TotalOrders { get; set; }
    public decimal TotalRevenue { get; set; }
}