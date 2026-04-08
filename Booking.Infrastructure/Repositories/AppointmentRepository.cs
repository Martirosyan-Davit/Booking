using Booking.Application.Interfaces.Repositories;
using Booking.Domain.Entities;
using Booking.Domain.Enums;
using Booking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Booking.Infrastructure.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly ApplicationDbContext _context;

    public AppointmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(List<AppointmentEntity> Items, int TotalCount)> GetAllAsync(
        DateTime? date = null,
        int? stylistId = null,
        AppointmentStatus? status = null,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Appointments
            .Include(a => a.Stylist)
            .Include(a => a.Service)
            .AsNoTracking()
            .AsQueryable();

        if (date.HasValue)
        {
            var dateValue = date.Value.Date;
            query = query.Where(a => a.StartsAt.Date == dateValue);
        }

        if (stylistId.HasValue)
            query = query.Where(a => a.StylistId == stylistId.Value);

        if (status.HasValue)
            query = query.Where(a => a.Status == status.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(a => a.StartsAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<AppointmentEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Appointments
            .Include(a => a.Stylist)
            .Include(a => a.Service)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task AddAsync(AppointmentEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.Appointments.AddAsync(entity, cancellationToken);
    }

    public void Update(AppointmentEntity entity)
    {
        _context.Appointments.Update(entity);
    }

    public async Task<bool> HasOverlappingAppointmentAsync(
        int stylistId,
        DateTime startsAt,
        DateTime endsAt,
        int? excludeAppointmentId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Appointments
            .Where(a => a.StylistId == stylistId)
            .Where(a => a.Status == AppointmentStatus.Confirmed)
            .Where(a => startsAt < a.EndsAt && endsAt > a.StartsAt);

        if (excludeAppointmentId.HasValue)
            query = query.Where(a => a.Id != excludeAppointmentId.Value);

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> HasFutureAppointmentsForStylistAsync(
        int stylistId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Appointments
            .Where(a => a.StylistId == stylistId)
            .Where(a => a.StartsAt > DateTime.UtcNow)
            .Where(a => a.Status != AppointmentStatus.Cancelled)
            .AnyAsync(cancellationToken);
    }

    public async Task<bool> HasFutureAppointmentsForServiceAsync(
        int serviceId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Appointments
            .Where(a => a.ServiceId == serviceId)
            .Where(a => a.StartsAt > DateTime.UtcNow)
            .Where(a => a.Status != AppointmentStatus.Cancelled)
            .AnyAsync(cancellationToken);
    }
}
