using Booking.Application.Common.DTOs;
using Booking.Domain.Enums;

namespace Booking.Application.DTOs.Appointment;

/// <summary>
/// Request DTO for filtering and paginating appointments.
/// </summary>
public class GetAppointmentsRequestDto : PageOptionsDto
{
    /// <summary>
    /// Filter by date (matches StartsAt date part).
    /// </summary>
    public DateTime? Date { get; set; }

    /// <summary>
    /// Filter by stylist ID.
    /// </summary>
    public int? StylistId { get; set; }

    /// <summary>
    /// Filter by appointment status (Pending, Confirmed, Cancelled).
    /// </summary>
    public AppointmentStatus? Status { get; set; }
}
