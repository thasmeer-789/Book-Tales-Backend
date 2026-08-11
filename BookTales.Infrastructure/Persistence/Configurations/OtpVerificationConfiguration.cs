using BookTales.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookTales.Infrastructure.Persistence.Configurations;

public class OtpVerificationConfiguration
    : IEntityTypeConfiguration<OtpVerification>
{
    public void Configure(
        EntityTypeBuilder<OtpVerification> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.CodeHash)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(o => o.Purpose)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(o => o.ExpiresAt)
               .IsRequired();

        builder.Property(o => o.IsUsed)
               .IsRequired();

        builder.HasOne(o => o.User)
               .WithMany()
               .HasForeignKey(o => o.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}