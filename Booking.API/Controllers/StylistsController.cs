using Booking.API.Wrappers;
using Booking.Application.DTOs.Stylist;
using Booking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers;

/// <summary>
/// Manages stylist resources in the salon.
/// </summary>
/// <remarks>
/// Stylists have working hours (WorkStart / WorkEnd) and a specialization.
/// A stylist cannot be deleted if they have future non-cancelled appointments.
/// </remarks>
[ApiController]
[Route("[controller]")]
public class StylistsController : ControllerBase
{
    private readonly IStylistService _stylistService;

    public StylistsController(IStylistService stylistService)
    {
        _stylistService = stylistService;
    }

    /// <summary>
    /// Returns all stylists.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResultContainer<List<StylistDto>>), 200)]
    public async Task<ActionResult<ApiResultContainer<List<StylistDto>>>> GetAll(CancellationToken cancellationToken)
    {
        var stylistDtos = await _stylistService.GetAllAsync(cancellationToken);
        return this.Success(stylistDtos);
    }

    /// <summary>
    /// Returns a single stylist by ID.
    /// </summary>
    /// <param name="id">Stylist identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResultContainer<StylistDto>), 200)]
    [ProducesResponseType(typeof(ApiFailResultContainer), 404)]
    public async Task<ActionResult<ApiResultContainer<StylistDto>>> GetById(int id, CancellationToken cancellationToken)
    {
        var stylistDto = await _stylistService.GetByIdAsync(id, cancellationToken);
        return this.Success(stylistDto);
    }

    /// <summary>
    /// Creates a new stylist.
    /// </summary>
    /// <param name="dto">Stylist creation payload.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResultContainer<StylistDto>), 201)]
    [ProducesResponseType(typeof(ApiFailResultContainer), 422)]
    public async Task<ActionResult<ApiResultContainer<StylistDto>>> Create(
        [FromBody] CreateStylistDto dto,
        CancellationToken cancellationToken)
    {
        var stylistDto = await _stylistService.CreateAsync(dto, cancellationToken);
        return this.Created(stylistDto);
    }

    /// <summary>
    /// Fully updates an existing stylist.
    /// </summary>
    /// <param name="id">Stylist identifier.</param>
    /// <param name="dto">Stylist update payload.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResultContainer<StylistDto>), 200)]
    [ProducesResponseType(typeof(ApiFailResultContainer), 404)]
    [ProducesResponseType(typeof(ApiFailResultContainer), 422)]
    public async Task<ActionResult<ApiResultContainer<StylistDto>>> Update(
        int id,
        [FromBody] UpdateStylistDto dto,
        CancellationToken cancellationToken)
    {
        var stylistDto = await _stylistService.UpdateAsync(id, dto, cancellationToken);
        return this.Success(stylistDto);
    }

    /// <summary>
    /// Deletes a stylist.
    /// </summary>
    /// <remarks>
    /// Only allowed if the stylist has no future non-cancelled appointments.
    /// Returns 409 Conflict otherwise.
    /// </remarks>
    /// <param name="id">Stylist identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResultContainer), 202)]
    [ProducesResponseType(typeof(ApiFailResultContainer), 404)]
    [ProducesResponseType(typeof(ApiFailResultContainer), 409)]
    public async Task<ActionResult<ApiResultContainer>> Delete(int id, CancellationToken cancellationToken)
    {
        await _stylistService.DeleteAsync(id, cancellationToken);
        return this.Success();
    }
}
