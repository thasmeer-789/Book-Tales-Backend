using BookTales.Domain.Common;

namespace BookTales.Domain.Entities;

public class OtpVerification : BaseEntity
{
    public Guid UserId { get; set; }

    public string CodeHash { get; set; } = string.Empty;

    public string Purpose { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public bool IsUsed { get; set; }

    public User User { get; set; } = null!;
}