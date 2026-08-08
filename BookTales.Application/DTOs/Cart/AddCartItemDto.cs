namespace BookTales.Application.DTOs.Cart;

public class AddCartItemDto
{
    public Guid BookId { get; set; }

    public int Quantity { get; set; } = 1;
}