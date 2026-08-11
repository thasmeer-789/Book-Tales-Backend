using System.ComponentModel.DataAnnotations;

namespace BookTales.Application.DTOs.Auth;

public class ForgotPasswordRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}