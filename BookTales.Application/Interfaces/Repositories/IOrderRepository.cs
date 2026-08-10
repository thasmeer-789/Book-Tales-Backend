using BookTales.Domain.Entities;

namespace BookTales.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> CreateAsync(Order order);

        Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId);

        Task<Order?> GetByIdAsync(Guid orderId);

        Task UpdateAsync(Order order);
    }
}