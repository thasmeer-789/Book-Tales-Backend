using BookTales.Domain.Common;

namespace BookTales.Domain.Entities
{
    public class Wishlist : BaseEntity
    {
        public Guid UserId { get; set; }

        public User? User { get; set; }

        public ICollection<WishlistItem> WishlistItems { get; set; } = new List<WishlistItem>();
    }
}