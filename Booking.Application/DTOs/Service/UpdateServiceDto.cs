namespace Booking.Application.DTOs.Service;

public class UpdateServiceDto
{
    public required string Name { get; set; }
    public int DurationMinutes { get; set; }
    public decimal Price { get; set; }
}
