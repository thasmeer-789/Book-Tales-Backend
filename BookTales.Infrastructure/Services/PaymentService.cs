using BookTales.Application.DTOs.Payment;
using BookTales.Application.Interfaces.Repositories;
using BookTales.Application.Interfaces.Services;
using BookTales.Domain.Enums;
using BookTales.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Razorpay.Api;
using System.Security.Cryptography;
using System.Text;

namespace BookTales.Infrastructure.Services;

public class PaymentService : IPaymentService
{
    private readonly RazorpaySettings _settings;
    private readonly IOrderRepository _orderRepository;

    public PaymentService(
        IOptions<RazorpaySettings> settings,
        IOrderRepository orderRepository)
    {
        _settings = settings.Value;
        _orderRepository = orderRepository;
    }

    public async Task<CreatePaymentOrderResponseDto> CreatePaymentOrderAsync(
        CreatePaymentOrderDto dto,
        Guid userId)
    {
        var order = await _orderRepository.GetByIdAsync(dto.OrderId);

        if (order == null)
        {
            throw new KeyNotFoundException("Order not found.");
        }

        if (order.UserId != userId)
        {
            throw new UnauthorizedAccessException(
                "You are not allowed to pay for this order.");
        }

        if (order.PaymentStatus != PaymentStatus.Pending)
        {
            throw new InvalidOperationException(
                "This order has already been processed.");
        }

        if (string.IsNullOrWhiteSpace(_settings.KeyId) ||
            string.IsNullOrWhiteSpace(_settings.KeySecret))
        {
            throw new InvalidOperationException(
                "Razorpay credentials are not configured.");
        }

        var client = new RazorpayClient(
            _settings.KeyId,
            _settings.KeySecret);

        var amountInPaise = (int)(order.TotalAmount * 100);

        var options = new Dictionary<string, object>
        {
            ["amount"] = amountInPaise,
            ["currency"] = "INR",
            ["receipt"] = order.Id.ToString(),
            ["payment_capture"] = 1
        };

        var razorpayOrder = client.Order.Create(options);

        // IMPORTANT:
        // Explicitly declare this as string? because Razorpay returns dynamic.
        string? razorpayOrderId =
            razorpayOrder["id"]?.ToString();

        if (string.IsNullOrWhiteSpace(razorpayOrderId))
        {
            throw new InvalidOperationException(
                "Razorpay order ID was not returned.");
        }

        // At this point C# knows razorpayOrderId is not null.
        order.RazorpayOrderId = razorpayOrderId;

        await _orderRepository.UpdateAsync(order);

        return new CreatePaymentOrderResponseDto
        {
            OrderId = order.Id,
            RazorpayOrderId = razorpayOrderId,
            KeyId = _settings.KeyId,
            Amount = order.TotalAmount,
            Currency = "INR"
        };
    }

    public async Task<bool> VerifyPaymentAsync(
        VerifyPaymentDto dto,
        Guid userId)
    {
        var order = await _orderRepository.GetByIdAsync(dto.OrderId);

        if (order == null)
        {
            throw new KeyNotFoundException("Order not found.");
        }

        if (order.UserId != userId)
        {
            throw new UnauthorizedAccessException(
                "You are not allowed to verify this payment.");
        }

        if (order.PaymentStatus != PaymentStatus.Pending)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(order.RazorpayOrderId) ||
            order.RazorpayOrderId != dto.RazorpayOrderId)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(dto.RazorpayPaymentId) ||
            string.IsNullOrWhiteSpace(dto.RazorpaySignature))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(_settings.KeySecret))
        {
            throw new InvalidOperationException(
                "Razorpay secret is not configured.");
        }

        var payload =
            $"{dto.RazorpayOrderId}|{dto.RazorpayPaymentId}";

        using var hmac = new HMACSHA256(
            Encoding.UTF8.GetBytes(_settings.KeySecret));

        var hash = hmac.ComputeHash(
            Encoding.UTF8.GetBytes(payload));

        var generatedSignature =
            Convert.ToHexString(hash).ToLowerInvariant();

        var isValid =
            CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(generatedSignature),
                Encoding.UTF8.GetBytes(dto.RazorpaySignature));

        if (!isValid)
        {
            return false;
        }

        order.RazorpayPaymentId = dto.RazorpayPaymentId;
        order.PaymentStatus = PaymentStatus.Paid;

        await _orderRepository.UpdateAsync(order);

        return true;
    }
}