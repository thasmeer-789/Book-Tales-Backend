namespace BookTales.Application.DTOs.Orders
{
    public class OrderItemDto
    {
        public Guid BookId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}