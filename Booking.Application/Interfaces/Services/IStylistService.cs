using Booking.Application.DTOs.Stylist;

namespace Booking.Application.Interfaces.Services;

public interface IStylistService
{
    Task<List<StylistDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<StylistDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<StylistDto> CreateAsync(CreateStylistDto dto, CancellationToken cancellationToken = default);
    Task<StylistDto> UpdateAsync(int id, UpdateStylistDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
