using Booking.Application.Common.Interfaces;
using Booking.Domain.Entities;

namespace Booking.Application.DTOs.Stylist;

public class StylistBriefDto : IAutoMap<StylistEntity, StylistBriefDto>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}
