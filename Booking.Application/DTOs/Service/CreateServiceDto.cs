namespace Booking.Application.DTOs.Service;

public class CreateServiceDto
{
    public required string Name { get; set; }
    public int DurationMinutes { get; set; }
    public decimal Price { get; set; }
}
