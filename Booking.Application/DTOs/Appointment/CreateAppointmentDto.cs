namespace Booking.Application.DTOs.Appointment;

public class CreateAppointmentDto
{
    public required string ClientName { get; set; }
    public int StylistId { get; set; }
    public int ServiceId { get; set; }
    public DateTime StartsAt { get; set; }
}
