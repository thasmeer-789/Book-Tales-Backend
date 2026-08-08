namespace BookTales.Application.DTOs.Book;

public class CreateBookDto
{
    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public string ISBN { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public DateTime PublishedDate { get; set; }

    public Guid CategoryId { get; set; }
}