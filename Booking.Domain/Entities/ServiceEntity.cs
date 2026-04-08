using Booking.Domain.Common;

namespace Booking.Domain.Entities;

public class ServiceEntity : AbstractEntity
{
    public string Name { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public decimal Price { get; set; }

    public ICollection<AppointmentEntity> Appointments { get; set; } = new List<AppointmentEntity>();
}
