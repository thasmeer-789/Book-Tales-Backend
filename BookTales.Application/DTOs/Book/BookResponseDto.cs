namespace BookTales.Application.DTOs.Book;

public class BookResponseDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public string ISBN { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public DateTime PublishedDate { get; set; }

    public Guid CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}