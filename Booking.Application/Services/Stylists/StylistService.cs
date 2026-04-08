using AutoMapper;
using Booking.Application.Common.Exceptions;
using Booking.Application.DTOs.Stylist;
using Booking.Application.Interfaces;
using Booking.Application.Interfaces.Repositories;
using Booking.Application.Interfaces.Services;
using Booking.Application.Services.Stylists.Exceptions;
using Booking.Domain.Entities;

namespace Booking.Application.Services.Stylists;

/// <summary>
/// Handles CRUD operations for stylists, including deletion guards.
/// </summary>
public class StylistService : IStylistService
{
    private readonly IStylistRepository _stylistRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StylistService(
        IStylistRepository stylistRepository,
        IAppointmentRepository appointmentRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _stylistRepository = stylistRepository;
        _appointmentRepository = appointmentRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Returns all active stylists.
    /// </summary>
    public async Task<List<StylistDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _stylistRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<StylistDto>>(entities);
    }

    /// <summary>
    /// Returns a single stylist by identifier.
    /// </summary>
    public async Task<StylistDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _stylistRepository.GetByIdAsync(id, cancellationToken);

        if (entity is null)
            throw new StylistNotFoundException();

        return _mapper.Map<StylistDto>(entity);
    }

    /// <summary>
    /// Creates a new stylist record.
    /// </summary>
    public async Task<StylistDto> CreateAsync(CreateStylistDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new StylistEntity
        {
            Name = dto.Name,
            Specialization = dto.Specialization,
            WorkStart = TimeOnly.Parse(dto.WorkStart),
            WorkEnd = TimeOnly.Parse(dto.WorkEnd),
            CreatedAt = DateTime.UtcNow
        };

        await _stylistRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<StylistDto>(entity);
    }

    /// <summary>
    /// Fully updates an existing stylist.
    /// </summary>
    public async Task<StylistDto> UpdateAsync(int id, UpdateStylistDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _stylistRepository.GetByIdAsync(id, cancellationToken);

        if (entity is null)
            throw new StylistNotFoundException();

        entity.Name = dto.Name;
        entity.Specialization = dto.Specialization;
        entity.WorkStart = TimeOnly.Parse(dto.WorkStart);
        entity.WorkEnd = TimeOnly.Parse(dto.WorkEnd);

        _stylistRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<StylistDto>(entity);
    }

    /// <summary>
    /// Deletes a stylist if they have no future non-cancelled appointments.
    /// </summary>
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _stylistRepository.GetByIdAsync(id, cancellationToken);

        if (entity is null)
            throw new StylistNotFoundException();

        var hasFutureAppointments = await _appointmentRepository
            .HasFutureAppointmentsForStylistAsync(id, cancellationToken);

        if (hasFutureAppointments)
            throw new ConflictException("Cannot delete stylist with future non-cancelled appointments.");

        _stylistRepository.Remove(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
