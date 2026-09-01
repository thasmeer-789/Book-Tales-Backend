namespace BookTales.Application.DTOs.Payment
{
    public class CreatePaymentOrderResponseDto
    {
        public Guid OrderId { get; set; }

        public string RazorpayOrderId { get; set; } = string.Empty;

        public string KeyId { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "INR";
    }
}