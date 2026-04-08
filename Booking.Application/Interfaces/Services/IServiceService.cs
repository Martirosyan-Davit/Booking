using Booking.Application.DTOs.Service;

namespace Booking.Application.Interfaces.Services;

public interface IServiceService
{
    Task<List<ServiceDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ServiceDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ServiceDto> CreateAsync(CreateServiceDto dto, CancellationToken cancellationToken = default);
    Task<ServiceDto> UpdateAsync(int id, UpdateServiceDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
