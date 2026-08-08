namespace BookTales.Application.DTOs.Wishlist;

public class WishlistResponseDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public List<WishlistItemResponseDto> Items { get; set; } = new();
}