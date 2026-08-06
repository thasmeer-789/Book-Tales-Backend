using BookTales.Domain.Common;

namespace BookTales.Domain.Entities
{
    public class CartItem : BaseEntity
    {
        public Guid CartId { get; set; }

        public Guid BookId { get; set; }

        public int Quantity { get; set; }

        public Cart? Cart { get; set; }

        public Book? Book { get; set; }
    }
}