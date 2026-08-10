using BookTales.Domain.Enums;

namespace BookTales.Application.DTOs.Orders
{
    public class OrderDto
    {
        public Guid Id { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public List<OrderItemDto> OrderItems { get; set; } = new();
    }
}