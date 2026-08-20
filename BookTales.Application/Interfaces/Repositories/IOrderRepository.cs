using BookTales.Domain.Entities;

namespace BookTales.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> CreateAsync(Order order);

        Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId);

        Task<Order?> GetByIdAsync(Guid orderId);

        Task<IEnumerable<Order>> GetAllAsync();
        
        Task UpdateAsync(Order order);

    }
}