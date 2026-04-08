using System.Net;

namespace Booking.Application.Common.Exceptions;

public class UnprocessableEntityException : BaseRequestException
{
    public UnprocessableEntityException()
        : base(HttpStatusCode.UnprocessableEntity)
    {
    }

    public UnprocessableEntityException(string message)
        : base(message, HttpStatusCode.UnprocessableEntity)
    {
    }
}
