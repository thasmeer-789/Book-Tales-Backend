using BookTales.Domain.Enums;

namespace BookTales.Application.DTOs.Orders
{
    public class UpdatePaymentStatusDto
    {
        public PaymentStatus PaymentStatus { get; set; }
    }
}