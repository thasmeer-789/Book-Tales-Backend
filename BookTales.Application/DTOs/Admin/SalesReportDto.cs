namespace BookTales.Application.DTOs.Admin;

public class SalesReportDto
{
    public decimal TotalRevenue { get; set; }
    public int TotalOrders { get; set; }
    public int PaidOrders { get; set; }
    public decimal AverageOrderValue { get; set; }
}