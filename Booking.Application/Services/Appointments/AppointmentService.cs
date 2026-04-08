using AutoMapper;
using Booking.Application.Common.DTOs;
using Booking.Application.Common.Exceptions;
using Booking.Application.DTOs.Appointment;
using Booking.Application.Interfaces;
using Booking.Application.Interfaces.Repositories;
using Booking.Application.Interfaces.Services;
using Booking.Application.Services.Appointments.Exceptions;
using Booking.Application.Services.Exceptions;
using Booking.Application.Services.Stylists.Exceptions;
using Booking.Domain.Entities;
using Booking.Domain.Enums;

namespace Booking.Application.Services.Appointments;

/// <summary>
/// Handles appointment booking, status transitions, and soft-delete operations.
/// </summary>
public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IStylistRepository _stylistRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AppointmentService(
        IAppointmentRepository appointmentRepository,
        IStylistRepository stylistRepository,
        IServiceRepository serviceRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _appointmentRepository = appointmentRepository;
        _stylistRepository = stylistRepository;
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Returns a paginated and filtered list of appointments.
    /// Soft-deleted appointments are excluded by the global query filter.
    /// </summary>
    public async Task<PageDto<AppointmentDto>> GetAllAsync(
        GetAppointmentsRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var (entities, totalCount) = await _appointmentRepository.GetAllAsync(
            request.Date,
            request.StylistId,
            request.Status,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var items = _mapper.Map<List<AppointmentDto>>(entities);

        return new PageDto<AppointmentDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    /// <summary>
    /// Returns a single appointment by its identifier, including stylist and service details.
    /// </summary>
    public async Task<AppointmentDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _appointmentRepository.GetByIdAsync(id, cancellationToken);

        if (entity is null)
            throw new AppointmentNotFoundException();

        return _mapper.Map<AppointmentDto>(entity);
    }

    /// <summary>
    /// Books a new appointment after verifying the stylist and service exist,
    /// the start time is in the future, and there are no scheduling conflicts.
    /// </summary>
    public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.StartsAt <= DateTime.UtcNow)
            throw new BadRequestException("StartsAt must be in the future.");

        var stylist = await _stylistRepository.GetByIdAsync(dto.StylistId, cancellationToken);

        if (stylist is null)
            throw new StylistNotFoundException();

        var service = await _serviceRepository.GetByIdAsync(dto.ServiceId, cancellationToken);

        if (service is null)
            throw new ServiceNotFoundException();

        var endsAt = dto.StartsAt.AddMinutes(service.DurationMinutes);

        var hasOverlap = await _appointmentRepository
            .HasOverlappingAppointmentAsync(dto.StylistId, dto.StartsAt, endsAt, null, cancellationToken);

        if (hasOverlap)
            throw new ConflictException("Stylist already has an appointment at this time.");

        var entity = new AppointmentEntity
        {
            ClientName = dto.ClientName,
            StylistId = dto.StylistId,
            ServiceId = dto.ServiceId,
            StartsAt = dto.StartsAt,
            EndsAt = endsAt,
            Status = AppointmentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _appointmentRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        entity.Stylist = stylist;
        entity.Service = service;

        return _mapper.Map<AppointmentDto>(entity);
    }

    /// <summary>
    /// Transitions the appointment status according to allowed rules:
    /// Pending -> Confirmed, Pending -> Cancelled, Confirmed -> Cancelled.
    /// Re-checks for conflicts when confirming.
    /// </summary>
    public async Task<AppointmentDto> UpdateStatusAsync(
        int id,
        UpdateAppointmentStatusDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await _appointmentRepository.GetByIdAsync(id, cancellationToken);

        if (entity is null)
            throw new AppointmentNotFoundException();

        var newStatus = Enum.Parse<AppointmentStatus>(dto.Status);

        ValidateStatusTransition(entity.Status, newStatus);

        if (newStatus == AppointmentStatus.Confirmed)
        {
            var hasOverlap = await _appointmentRepository
                .HasOverlappingAppointmentAsync(entity.StylistId, entity.StartsAt, entity.EndsAt, entity.Id, cancellationToken);

            if (hasOverlap)
                throw new ConflictException("Stylist already has a confirmed appointment at this time.");
        }

        entity.Status = newStatus;

        _appointmentRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AppointmentDto>(entity);
    }

    /// <summary>
    /// Soft-deletes an appointment by setting DeletedAt to the current UTC timestamp.
    /// The row is never physically removed from the database.
    /// </summary>
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _appointmentRepository.GetByIdAsync(id, cancellationToken);

        if (entity is null)
            throw new AppointmentNotFoundException();

        entity.DeletedAt = DateTime.UtcNow;

        _appointmentRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static void ValidateStatusTransition(AppointmentStatus currentStatus, AppointmentStatus newStatus)
    {
        var isValid = (currentStatus, newStatus) switch
        {
            (AppointmentStatus.Pending, AppointmentStatus.Confirmed) => true,
            (AppointmentStatus.Pending, AppointmentStatus.Cancelled) => true,
            (AppointmentStatus.Confirmed, AppointmentStatus.Cancelled) => true,
            _ => false
        };

        if (!isValid)
            throw new UnprocessableEntityException(
                $"Cannot transition from '{currentStatus}' to '{newStatus}'.");
    }
}
