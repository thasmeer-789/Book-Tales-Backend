using System.ComponentModel.DataAnnotations;

namespace BookTales.Application.DTOs.Orders
{
    public class CreateOrderDto
    {
        public Guid UserId { get; set; }

        [Required]
        [MinLength(1)]
        public List<OrderItemDto> OrderItems { get; set; } = new();
    }
}