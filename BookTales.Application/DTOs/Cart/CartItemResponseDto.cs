namespace BookTales.Application.DTOs.Cart;

public class CartItemResponseDto
{
    public Guid Id { get; set; }

    public Guid BookId { get; set; }

    public string BookTitle { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public decimal Subtotal { get; set; }

    public string ImageUrl { get; set; } = string.Empty;
}