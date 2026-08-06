using BookTales.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookTales.Infrastructure.Persistence.Configurations
{
    public class WishlistItemConfiguration : IEntityTypeConfiguration<WishlistItem>
    {
        public void Configure(EntityTypeBuilder<WishlistItem> builder)
        {
            builder.HasKey(wi => wi.Id);

            builder.HasOne(wi => wi.Book)
                   .WithMany(b => b.WishlistItems)
                   .HasForeignKey(wi => wi.BookId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}