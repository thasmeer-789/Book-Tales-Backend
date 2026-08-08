namespace BookTales.Application.DTOs.Wishlist;

public class WishlistItemResponseDto
{
    public Guid Id { get; set; }

    public Guid BookId { get; set; }

    public string BookTitle { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string ImageUrl { get; set; } = string.Empty;
}
