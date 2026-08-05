using Microsoft.EntityFrameworkCore;

namespace BookTales.Infrastructure.Data
{
    public class BookTalesDbContext : DbContext
    {
        public BookTalesDbContext(DbContextOptions<BookTalesDbContext> options)
            : base(options)
        {
        }
    }
}