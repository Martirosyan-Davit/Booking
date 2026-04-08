using Booking.Domain.Common;

namespace Booking.Domain.Entities;

public class StylistEntity : AbstractEntity
{
    public string Name { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public TimeOnly WorkStart { get; set; }
    public TimeOnly WorkEnd { get; set; }

    public ICollection<AppointmentEntity> Appointments { get; set; } = new List<AppointmentEntity>();
}
