using System.Net;
using Booking.Application.Common.Exceptions;

namespace Booking.Application.Services.Appointments.Exceptions;

public class AppointmentNotFoundException : BaseRequestException
{
    public AppointmentNotFoundException()
        : base("appointment.notFound", HttpStatusCode.NotFound)
    {
    }
}
