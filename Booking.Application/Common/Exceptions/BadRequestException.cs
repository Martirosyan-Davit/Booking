using System.Net;

namespace Booking.Application.Common.Exceptions;

public class BadRequestException : BaseRequestException
{
    public BadRequestException(string message)
        : base(message, HttpStatusCode.BadRequest)
    {
    }
}
