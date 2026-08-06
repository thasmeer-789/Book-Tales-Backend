using BookTales.Domain.Common;

namespace BookTales.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public Guid OrderId { get; set; }

        public Guid BookId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public Order? Order { get; set; }

        public Book? Book { get; set; }
    }
}