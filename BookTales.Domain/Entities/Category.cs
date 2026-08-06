using BookTales.Domain.Common;

namespace BookTales.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}