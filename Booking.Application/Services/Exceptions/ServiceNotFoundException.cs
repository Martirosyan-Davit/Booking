using System.Net;
using Booking.Application.Common.Exceptions;

namespace Booking.Application.Services.Exceptions;

public class ServiceNotFoundException : BaseRequestException
{
    public ServiceNotFoundException()
        : base("service.notFound", HttpStatusCode.NotFound)
    {
    }
}
