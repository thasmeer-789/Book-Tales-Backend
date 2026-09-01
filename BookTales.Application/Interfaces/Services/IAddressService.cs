using BookTales.Application.DTOs.Address;

namespace BookTales.Application.Interfaces.Services;

public interface IAddressService
{
    Task<IEnumerable<AddressDto>> GetMyAddressesAsync(Guid userId);

    Task<AddressDto?> GetAddressByIdAsync(
        Guid id,
        Guid userId);

    Task<AddressDto> CreateAddressAsync(
        Guid userId,
        CreateUpdateAddressDto dto);

    Task<AddressDto?> UpdateAddressAsync(
        Guid id,
        Guid userId,
        CreateUpdateAddressDto dto);

    Task<bool> DeleteAddressAsync(
        Guid id,
        Guid userId);
}