using BookTales.Domain.Enums;

namespace BookTales.Application.DTOs.Orders
{
    public class UpdateOrderStatusDto
    {
        public OrderStatus Status { get; set; }
    }
}