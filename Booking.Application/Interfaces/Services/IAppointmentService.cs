using Booking.Application.Common.DTOs;
using Booking.Application.DTOs.Appointment;

namespace Booking.Application.Interfaces.Services;

public interface IAppointmentService
{
    Task<PageDto<AppointmentDto>> GetAllAsync(
        GetAppointmentsRequestDto request,
        CancellationToken cancellationToken = default);

    Task<AppointmentDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto, CancellationToken cancellationToken = default);
    Task<AppointmentDto> UpdateStatusAsync(int id, UpdateAppointmentStatusDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
