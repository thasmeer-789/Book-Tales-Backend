using BookTales.Application.DTOs.Orders;

namespace BookTales.Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(CreateOrderDto dto);

        Task<IEnumerable<OrderDto>> GetMyOrdersAsync(Guid userId);

        Task<OrderDto?> GetOrderByIdAsync(Guid orderId, Guid userId);

        Task<OrderDto?> UpdateOrderStatusAsync(
            Guid orderId,
            UpdateOrderStatusDto dto);

        Task<OrderDto?> UpdatePaymentStatusAsync(
            Guid orderId,
            UpdatePaymentStatusDto dto);
    }
}