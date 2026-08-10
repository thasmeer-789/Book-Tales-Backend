using System.ComponentModel.DataAnnotations;

namespace BookTales.Application.DTOs.Orders
{
    public class OrderItemDto
    {
        public Guid BookId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}