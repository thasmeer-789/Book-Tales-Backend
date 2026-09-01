using BookTales.Application.DTOs.Payment;

namespace BookTales.Application.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<CreatePaymentOrderResponseDto> CreatePaymentOrderAsync(
            CreatePaymentOrderDto dto,
            Guid userId);

        Task<bool> VerifyPaymentAsync(
            VerifyPaymentDto dto,
            Guid userId);
    }
}