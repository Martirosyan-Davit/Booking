using Booking.API.Wrappers;
using Booking.Application.Common.DTOs;
using Booking.Application.DTOs.Appointment;
using Booking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers;

/// <summary>
/// Manages appointment booking, status transitions, and soft-delete.
/// </summary>
/// <remarks>
/// Appointments link a client to a stylist and a service at a given time slot.
/// Conflict detection prevents double-booking a stylist.
/// Soft-deleted appointments are excluded from all list queries.
/// </remarks>
[ApiController]
[Route("[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    /// <summary>
    /// Returns a paginated list of appointments with optional filters.
    /// </summary>
    /// <remarks>
    /// Supports filtering by date, stylistId, and status via query parameters.
    /// Soft-deleted appointments are automatically excluded.
    /// </remarks>
    /// <param name="request">Filtering and pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResultContainer<PageDto<AppointmentDto>>), 200)]
    [ProducesResponseType(typeof(ApiFailResultContainer), 422)]
    public async Task<ActionResult<ApiResultContainer<PageDto<AppointmentDto>>>> GetAll(
        [FromQuery] GetAppointmentsRequestDto request,
        CancellationToken cancellationToken)
    {
        var pageAppointmentDto = await _appointmentService.GetAllAsync(request, cancellationToken);
        return this.Success(pageAppointmentDto);
    }

    /// <summary>
    /// Returns a single appointment by ID, including stylist and service details.
    /// </summary>
    /// <param name="id">Appointment identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResultContainer<AppointmentDto>), 200)]
    [ProducesResponseType(typeof(ApiFailResultContainer), 404)]
    public async Task<ActionResult<ApiResultContainer<AppointmentDto>>> GetById(int id, CancellationToken cancellationToken)
    {
        var appointmentDto = await _appointmentService.GetByIdAsync(id, cancellationToken);
        return this.Success(appointmentDto);
    }

    /// <summary>
    /// Books a new appointment.
    /// </summary>
    /// <remarks>
    /// The appointment is created with Pending status.
    /// Returns 400 if StartsAt is in the past,
    /// 404 if the stylist or service does not exist,
    /// 409 if the stylist has a conflicting confirmed appointment.
    /// </remarks>
    /// <param name="dto">Appointment creation payload.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResultContainer<AppointmentDto>), 201)]
    [ProducesResponseType(typeof(ApiFailResultContainer), 400)]
    [ProducesResponseType(typeof(ApiFailResultContainer), 404)]
    [ProducesResponseType(typeof(ApiFailResultContainer), 409)]
    [ProducesResponseType(typeof(ApiFailResultContainer), 422)]
    public async Task<ActionResult<ApiResultContainer<AppointmentDto>>> Create(
        [FromBody] CreateAppointmentDto dto,
        CancellationToken cancellationToken)
    {
        var appointmentDto = await _appointmentService.CreateAsync(dto, cancellationToken);
        return this.Created(appointmentDto);
    }

    /// <summary>
    /// Changes the status of an existing appointment.
    /// </summary>
    /// <remarks>
    /// Allowed transitions: Pending -> Confirmed, Pending -> Cancelled, Confirmed -> Cancelled.
    /// Returns 409 if confirming would cause a scheduling conflict,
    /// 422 if the transition is not allowed.
    /// </remarks>
    /// <param name="id">Appointment identifier.</param>
    /// <param name="dto">New status payload.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpPatch("{id:int}/status")]
    [ProducesResponseType(typeof(ApiResultContainer<AppointmentDto>), 200)]
    [ProducesResponseType(typeof(ApiFailResultContainer), 404)]
    [ProducesResponseType(typeof(ApiFailResultContainer), 409)]
    [ProducesResponseType(typeof(ApiFailResultContainer), 422)]
    public async Task<ActionResult<ApiResultContainer<AppointmentDto>>> UpdateStatus(
        int id,
        [FromBody] UpdateAppointmentStatusDto dto,
        CancellationToken cancellationToken)
    {
        var updatedAppointmentDto = await _appointmentService.UpdateStatusAsync(id, dto, cancellationToken);
        return this.Success(updatedAppointmentDto);
    }

    /// <summary>
    /// Soft-deletes an appointment.
    /// </summary>
    /// <remarks>
    /// Sets DeletedAt to the current UTC timestamp.
    /// The row is never physically removed from the database.
    /// </remarks>
    /// <param name="id">Appointment identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResultContainer), 202)]
    [ProducesResponseType(typeof(ApiFailResultContainer), 404)]
    public async Task<ActionResult<ApiResultContainer>> Delete(int id, CancellationToken cancellationToken)
    {
        await _appointmentService.DeleteAsync(id, cancellationToken);
        return this.Success();
    }
}
