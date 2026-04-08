using System.Net;
using Booking.Application.Common.Exceptions;

namespace Booking.Application.Services.Stylists.Exceptions;

public class StylistNotFoundException : BaseRequestException
{
    public StylistNotFoundException()
        : base("stylist.notFound", HttpStatusCode.NotFound)
    {
    }
}