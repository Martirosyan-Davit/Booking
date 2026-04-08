using Booking.Domain.Common;
using Booking.Domain.Enums;

namespace Booking.Domain.Entities;

public class AppointmentEntity : AbstractEntity
{
    public string ClientName { get; set; } = string.Empty;
    public int StylistId { get; set; }
    public int ServiceId { get; set; }

    /// <remarks>
    /// In general, it makes sense to abstract such audit columns and inherit them 
    /// through the audit interface to make it easier to collect audits later, 
    /// or to integrate interceptors for collecting audits
    /// </remarks>
    public DateTime StartsAt { get; set; }

    /// <remarks>
    /// In general, it makes sense to abstract such audit columns and inherit them 
    /// through the audit interface to make it easier to collect audits later, 
    /// or to integrate interceptors for collecting audits
    /// </remarks>
    public DateTime EndsAt { get; set; }
    public AppointmentStatus Status { get; set; }

    /// <remarks>
    /// In general, it makes sense to abstract such audit columns and inherit them 
    /// through the audit interface to make it easier to collect audits later, 
    /// or to integrate interceptors for collecting audits
    /// </remarks>
    public DateTime? DeletedAt { get; set; }

    public StylistEntity Stylist { get; set; } = null!;
    public ServiceEntity Service { get; set; } = null!;
}
