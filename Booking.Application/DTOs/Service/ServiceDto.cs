using Booking.Application.Common.Interfaces;
using Booking.Domain.Entities;

namespace Booking.Application.DTOs.Service;

public class ServiceDto : IAutoMap<ServiceEntity, ServiceDto>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int DurationMinutes { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
}
