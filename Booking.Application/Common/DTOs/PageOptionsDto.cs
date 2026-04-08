using Booking.Application.Common.Enums;

namespace Booking.Application.Common.DTOs;

/// <summary>
/// Base class for page options request
/// </summary>
public class PageOptionsDto
{
    /// <summary>
    /// Search text
    /// </summary>
    public string? Q { get; set; }

    /// <summary>
    /// Sorting page
    /// </summary>
    public Order Order { get; set; } = Order.DESC;

    /// <summary>
    /// Page number (starting from 1)
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Page size
    /// </summary>
    public int PageSize { get; set; } = 20;
}
