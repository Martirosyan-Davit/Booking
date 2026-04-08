using System.Net;

namespace Booking.Application.Common.Exceptions;

public class ConflictException : BaseRequestException
{
    public ConflictException()
        : base(HttpStatusCode.Conflict)
    {
    }

    public ConflictException(string message)
        : base(message, HttpStatusCode.Conflict)
    {
    }
}
