using Booking.Domain.Entities;
using Booking.Domain.Enums;

namespace Booking.Application.Interfaces.Repositories;

public interface IAppointmentRepository
{
    Task<(List<AppointmentEntity> Items, int TotalCount)> GetAllAsync(
        DateTime? date = null,
        int? stylistId = null,
        AppointmentStatus? status = null,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default);

    Task<AppointmentEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(AppointmentEntity entity, CancellationToken cancellationToken = default);
    void Update(AppointmentEntity entity);

    Task<bool> HasOverlappingAppointmentAsync(
        int stylistId,
        DateTime startsAt,
        DateTime endsAt,
        int? excludeAppointmentId = null,
        CancellationToken cancellationToken = default);

    Task<bool> HasFutureAppointmentsForStylistAsync(int stylistId, CancellationToken cancellationToken = default);
    Task<bool> HasFutureAppointmentsForServiceAsync(int serviceId, CancellationToken cancellationToken = default);
}
