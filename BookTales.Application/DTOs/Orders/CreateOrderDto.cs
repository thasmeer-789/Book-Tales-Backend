namespace BookTales.Application.DTOs.Orders
{
    public class CreateOrderDto
    {
        public Guid UserId { get; set; }

        public List<OrderItemDto> OrderItems { get; set; } = new();
    }
}