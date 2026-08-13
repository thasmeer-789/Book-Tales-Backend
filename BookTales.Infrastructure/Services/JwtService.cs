using BookTales.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookTales.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly string _key;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly double _expiryInMinutes;

    public JwtService(IConfiguration configuration)
    {
        _key = configuration["Jwt:Key"]
            ?? throw new InvalidOperationException(
                "JWT Key is not configured.");

        _issuer = configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException(
                "JWT Issuer is not configured.");

        _audience = configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException(
                "JWT Audience is not configured.");

        if (!double.TryParse(
                configuration["Jwt:ExpiryInMinutes"],
                out _expiryInMinutes))
        {
            throw new InvalidOperationException(
                "JWT ExpiryInMinutes is not configured correctly.");
        }

        if (Encoding.UTF8.GetByteCount(_key) < 32)
        {
            throw new InvalidOperationException(
                "JWT Key must be at least 32 bytes long.");
        }
    }

    public string GenerateToken(
        Guid userId,
        string email,
        IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new(
                ClaimTypes.NameIdentifier,
                userId.ToString()),

            new(
                ClaimTypes.Email,
                email)
        };

        foreach (var role in roles)
        {
            claims.Add(
                new Claim(
                    ClaimTypes.Role,
                    role));
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_key));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var expiresAt = DateTime.UtcNow.AddMinutes(
            _expiryInMinutes);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}