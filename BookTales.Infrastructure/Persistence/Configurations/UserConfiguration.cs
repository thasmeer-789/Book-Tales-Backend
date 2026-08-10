using BookTales.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookTales.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.FirstName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(u => u.LastName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasIndex(u => u.Email)
                   .IsUnique();

            builder.Property(u => u.PhoneNumber)
                   .HasMaxLength(15);

            // One User -> One Cart
            builder.HasOne(u => u.Cart)
                   .WithOne(c => c.User)
                   .HasForeignKey<Cart>(c => c.UserId);

            // One User -> One Wishlist
            builder.HasOne(u => u.Wishlist)
                   .WithOne(w => w.User)
                   .HasForeignKey<Wishlist>(w => w.UserId);

            // One User -> Many Orders
            builder.HasMany(u => u.Orders)
                   .WithOne(o => o.User)
                   .HasForeignKey(o => o.UserId);
        }
    }
}