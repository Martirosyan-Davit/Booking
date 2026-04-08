namespace Booking.Application.DTOs.Stylist;

public class UpdateStylistDto
{
    public required string Name { get; set; }
    public required string Specialization { get; set; }
    public required string WorkStart { get; set; }
    public required string WorkEnd { get; set; }
}
