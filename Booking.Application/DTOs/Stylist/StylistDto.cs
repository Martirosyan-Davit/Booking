using AutoMapper;
using Booking.Application.Common.Interfaces;
using Booking.Domain.Entities;

namespace Booking.Application.DTOs.Stylist;

public class StylistDto : IAutoMap<StylistEntity, StylistDto>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Specialization { get; set; } = null!;
    public string WorkStart { get; set; } = null!;
    public string WorkEnd { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public void CreateMap(Profile profile)
    {
        profile.CreateMap<StylistEntity, StylistDto>()
            .ForMember(d => d.WorkStart, opt => opt.MapFrom(s => s.WorkStart.ToString("HH:mm")))
            .ForMember(d => d.WorkEnd, opt => opt.MapFrom(s => s.WorkEnd.ToString("HH:mm")));
    }
}
