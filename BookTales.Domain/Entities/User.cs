using BookTales.Domain.Common;

namespace BookTales.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public bool IsBlocked { get; set; } = false;

        public Cart? Cart { get; set; }

        public Wishlist? Wishlist { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}