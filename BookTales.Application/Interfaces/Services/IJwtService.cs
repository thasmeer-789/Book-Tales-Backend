namespace BookTales.Application.Interfaces.Services;

public interface IJwtService
{
    string GenerateToken(
        Guid userId,
        string email,
        IList<string> roles);
}