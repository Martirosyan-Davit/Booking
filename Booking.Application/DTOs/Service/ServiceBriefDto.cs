using Booking.Application.Common.Interfaces;
using Booking.Domain.Entities;

namespace Booking.Application.DTOs.Service;

public class ServiceBriefDto : IAutoMap<ServiceEntity, ServiceBriefDto>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int DurationMinutes { get; set; }
    public decimal Price { get; set; }
}
