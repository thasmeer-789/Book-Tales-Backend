using BookTales.Application.DTOs.Address;
using BookTales.Application.Interfaces.Repositories;
using BookTales.Application.Interfaces.Services;
using BookTales.Domain.Entities;

namespace BookTales.Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(
            IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<IEnumerable<AddressDto>> GetMyAddressesAsync(
            Guid userId)
        {
            var addresses =
                await _addressRepository.GetByUserIdAsync(userId);

            return addresses.Select(MapToDto);
        }

        public async Task<AddressDto?> GetAddressByIdAsync(
            Guid id,
            Guid userId)
        {
            var address =
                await _addressRepository.GetByIdAsync(id, userId);

            if (address == null)
                return null;

            return MapToDto(address);
        }

        public async Task<AddressDto> CreateAddressAsync(
            Guid userId,
            CreateUpdateAddressDto dto)
        {
            var existingAddresses =
                await _addressRepository.GetByUserIdAsync(userId);

            var isFirstAddress = !existingAddresses.Any();

            if (isFirstAddress)
            {
                dto.IsDefault = true;
            }

            if (dto.IsDefault)
            {
                foreach (var existingAddress in existingAddresses)
                {
                    if (existingAddress.IsDefault)
                    {
                        existingAddress.IsDefault = false;

                        await _addressRepository.UpdateAsync(
                            existingAddress);
                    }
                }
            }

            var address = new Address
            {
                Id = Guid.NewGuid(),
                UserId = userId,

                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                AddressLine = dto.AddressLine,
                City = dto.City,
                State = dto.State,
                PostalCode = dto.PostalCode,
                Country = dto.Country,
                IsDefault = dto.IsDefault
            };

            var created =
                await _addressRepository.CreateAsync(address);

            return MapToDto(created);
        }

        public async Task<AddressDto?> UpdateAddressAsync(
            Guid id,
            Guid userId,
            CreateUpdateAddressDto dto)
        {
            var address =
                await _addressRepository.GetByIdAsync(id, userId);

            if (address == null)
                return null;

            if (dto.IsDefault)
            {
                var existingAddresses =
                    await _addressRepository.GetByUserIdAsync(userId);

                foreach (var existingAddress in existingAddresses)
                {
                    if (existingAddress.Id != id &&
                        existingAddress.IsDefault)
                    {
                        existingAddress.IsDefault = false;

                        await _addressRepository.UpdateAsync(
                            existingAddress);
                    }
                }
            }

            address.FullName = dto.FullName;
            address.PhoneNumber = dto.PhoneNumber;
            address.AddressLine = dto.AddressLine;
            address.City = dto.City;
            address.State = dto.State;
            address.PostalCode = dto.PostalCode;
            address.Country = dto.Country;
            address.IsDefault = dto.IsDefault;

            await _addressRepository.UpdateAsync(address);

            return MapToDto(address);
        }

        public async Task<bool> DeleteAddressAsync(
            Guid id,
            Guid userId)
        {
            var address =
                await _addressRepository.GetByIdAsync(id, userId);

            if (address == null)
                return false;

            var wasDefault = address.IsDefault;

            await _addressRepository.DeleteAsync(address);

            if (wasDefault)
            {
                var remainingAddresses =
                    await _addressRepository.GetByUserIdAsync(userId);

                var newDefault =
                    remainingAddresses.FirstOrDefault();

                if (newDefault != null)
                {
                    newDefault.IsDefault = true;

                    await _addressRepository.UpdateAsync(
                        newDefault);
                }
            }

            return true;
        }

        private static AddressDto MapToDto(Address address)
        {
            return new AddressDto
            {
                Id = address.Id,
                FullName = address.FullName,
                PhoneNumber = address.PhoneNumber,
                AddressLine = address.AddressLine,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode,
                Country = address.Country,
                IsDefault = address.IsDefault
            };
        }
    }
}