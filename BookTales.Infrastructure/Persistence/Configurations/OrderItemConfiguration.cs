using BookTales.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookTales.Infrastructure.Persistence.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.Quantity)
                   .IsRequired();

            builder.Property(oi => oi.Price)
                   .HasPrecision(18, 2);

            builder.HasOne(oi => oi.Book)
                   .WithMany(b => b.OrderItems)
                   .HasForeignKey(oi => oi.BookId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}