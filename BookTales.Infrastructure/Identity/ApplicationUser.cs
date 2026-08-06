using Microsoft.AspNetCore.Identity;

namespace BookTales.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public Guid DomainUserId { get; set; }
    }
}