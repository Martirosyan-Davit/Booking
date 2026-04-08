using AutoMapper;
using Booking.Application.Common.Exceptions;
using Booking.Application.DTOs.Service;
using Booking.Application.Interfaces;
using Booking.Application.Interfaces.Repositories;
using Booking.Application.Interfaces.Services;
using Booking.Application.Services.Exceptions;
using Booking.Domain.Entities;

namespace Booking.Application.Services;

/// <summary>
/// Handles CRUD operations for salon services, including deletion guards.
/// </summary>
public class ServiceService : IServiceService
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ServiceService(
        IServiceRepository serviceRepository,
        IAppointmentRepository appointmentRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _serviceRepository = serviceRepository;
        _appointmentRepository = appointmentRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Returns all available salon services.
    /// </summary>
    public async Task<List<ServiceDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _serviceRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<ServiceDto>>(entities);
    }

    /// <summary>
    /// Returns a single service by identifier.
    /// </summary>
    public async Task<ServiceDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _serviceRepository.GetByIdAsync(id, cancellationToken);

        if (entity is null)
            throw new ServiceNotFoundException();

        return _mapper.Map<ServiceDto>(entity);
    }

    /// <summary>
    /// Creates a new salon service record.
    /// </summary>
    public async Task<ServiceDto> CreateAsync(CreateServiceDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new ServiceEntity
        {
            Name = dto.Name,
            DurationMinutes = dto.DurationMinutes,
            Price = dto.Price,
            CreatedAt = DateTime.UtcNow
        };

        await _serviceRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ServiceDto>(entity);
    }

    /// <summary>
    /// Fully updates an existing salon service.
    /// </summary>
    public async Task<ServiceDto> UpdateAsync(int id, UpdateServiceDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _serviceRepository.GetByIdAsync(id, cancellationToken);

        if (entity is null)
            throw new ServiceNotFoundException();

        entity.Name = dto.Name;
        entity.DurationMinutes = dto.DurationMinutes;
        entity.Price = dto.Price;

        _serviceRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ServiceDto>(entity);
    }

    /// <summary>
    /// Deletes a service if it is not used in any future non-cancelled appointment.
    /// </summary>
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _serviceRepository.GetByIdAsync(id, cancellationToken);

        if (entity is null)
            throw new ServiceNotFoundException();

        var hasFutureAppointments = await _appointmentRepository
            .HasFutureAppointmentsForServiceAsync(id, cancellationToken);

        if (hasFutureAppointments)
            throw new ConflictException("Cannot delete service used in future non-cancelled appointments.");

        _serviceRepository.Remove(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
