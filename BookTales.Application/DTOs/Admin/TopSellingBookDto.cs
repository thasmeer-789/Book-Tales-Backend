namespace BookTales.Application.DTOs.Admin;

public class TopSellingBookDto
{
    public Guid? BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public int TotalQuantitySold { get; set; }
    public decimal TotalSales { get; set; }
}