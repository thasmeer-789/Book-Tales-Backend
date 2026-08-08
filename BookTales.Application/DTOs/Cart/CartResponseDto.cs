namespace BookTales.Application.DTOs.Cart;

public class CartResponseDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public List<CartItemResponseDto> Items { get; set; } = new();

    public decimal Total { get; set; }
}