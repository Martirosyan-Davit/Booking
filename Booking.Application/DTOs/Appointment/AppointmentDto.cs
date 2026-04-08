using AutoMapper;
using Booking.Application.Common.Interfaces;
using Booking.Application.DTOs.Service;
using Booking.Application.DTOs.Stylist;
using Booking.Domain.Entities;

namespace Booking.Application.DTOs.Appointment;

public class AppointmentDto : IAutoMap<AppointmentEntity, AppointmentDto>
{
    public int Id { get; set; }
    public string ClientName { get; set; } = null!;
    public StylistBriefDto Stylist { get; set; } = null!;
    public ServiceBriefDto Service { get; set; } = null!;
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public string Status { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public void CreateMap(Profile profile)
    {
        profile.CreateMap<AppointmentEntity, AppointmentDto>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));
    }
}
