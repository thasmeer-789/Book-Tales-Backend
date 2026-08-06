using BookTales.Domain.Common;

namespace BookTales.Domain.Entities
{
    public class WishlistItem : BaseEntity
    {
        public Guid WishlistId { get; set; }

        public Guid BookId { get; set; }

        public Wishlist? Wishlist { get; set; }

        public Book? Book { get; set; }
    }
}