using Booking.API.Wrappers;
using Booking.Application.DTOs.Service;
using Booking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers;

/// <summary>
/// Manages salon service resources (haircut, coloring, etc.).
/// </summary>
/// <remarks>
/// Each service has a name, duration in minutes, and a price.
/// A service cannot be deleted if it is used in any future non-cancelled appointment.
/// </remarks>
[ApiController]
[Route("[controller]")]
public class ServicesController : ControllerBase
{
    private readonly IServiceService _serviceService;

    public ServicesController(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    /// <summary>
    /// Returns all salon services.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResultContainer<List<ServiceDto>>), 200)]
    public async Task<ActionResult<ApiResultContainer<List<ServiceDto>>>> GetAll(CancellationToken cancellationToken)
    {
        var serviceDtos = await _serviceService.GetAllAsync(cancellationToken);
        return this.Success(serviceDtos);
    }

    /// <summary>
    /// Returns a single service by ID.
    /// </summary>
    /// <param name="id">Service identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResultContainer<ServiceDto>), 200)]
    [ProducesResponseType(typeof(ApiFailResultContainer), 404)]
    public async Task<ActionResult<ApiResultContainer<ServiceDto>>> GetById(int id, CancellationToken cancellationToken)
    {
        var serviceDto = await _serviceService.GetByIdAsync(id, cancellationToken);
        return this.Success(serviceDto);
    }

    /// <summary>
    /// Creates a new salon service.
    /// </summary>
    /// <param name="dto">Service creation payload.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResultContainer<ServiceDto>), 201)]
    [ProducesResponseType(typeof(ApiFailResultContainer), 422)]
    public async Task<ActionResult<ApiResultContainer<ServiceDto>>> Create(
        [FromBody] CreateServiceDto dto,
        CancellationToken cancellationToken)
    {
        var serviceDto = await _serviceService.CreateAsync(dto, cancellationToken);
        return this.Created(serviceDto);
    }

    /// <summary>
    /// Fully updates an existing salon service.
    /// </summary>
    /// <param name="id">Service identifier.</param>
    /// <param name="dto">Service update payload.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResultContainer<ServiceDto>), 200)]
    [ProducesResponseType(typeof(ApiFailResultContainer), 404)]
    [ProducesResponseType(typeof(ApiFailResultContainer), 422)]
    public async Task<ActionResult<ApiResultContainer<ServiceDto>>> Update(
        int id,
        [FromBody] UpdateServiceDto dto,
        CancellationToken cancellationToken)
    {
        var updatedServiceDto = await _serviceService.UpdateAsync(id, dto, cancellationToken);
        return this.Success(updatedServiceDto);
    }

    /// <summary>
    /// Deletes a salon service.
    /// </summary>
    /// <remarks>
    /// Only allowed if the service is not used in any future non-cancelled appointment.
    /// Returns 409 Conflict otherwise.
    /// </remarks>
    /// <param name="id">Service identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResultContainer), 202)]
    [ProducesResponseType(typeof(ApiFailResultContainer), 404)]
    [ProducesResponseType(typeof(ApiFailResultContainer), 409)]
    public async Task<ActionResult<ApiResultContainer>> Delete(int id, CancellationToken cancellationToken)
    {
        await _serviceService.DeleteAsync(id, cancellationToken);
        return this.Success();
    }
}
