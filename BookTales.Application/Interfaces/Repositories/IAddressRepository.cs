using BookTales.Domain.Entities;

namespace BookTales.Application.Interfaces.Repositories;

public interface IAddressRepository
{
    Task<IEnumerable<Address>> GetByUserIdAsync(Guid userId);

    Task<Address?> GetByIdAsync(Guid id, Guid userId);

    Task<Address> CreateAsync(Address address);

    Task UpdateAsync(Address address);

    Task DeleteAsync(Address address);
}