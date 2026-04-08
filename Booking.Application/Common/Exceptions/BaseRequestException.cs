using System.Net;

namespace Booking.Application.Common.Exceptions;

public class BaseRequestException : ApplicationException
{
    public HttpStatusCode StatusCode { get; } = HttpStatusCode.InternalServerError;

    public BaseRequestException()
    {
    }

    public BaseRequestException(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }

    public BaseRequestException(string message, HttpStatusCode statusCode)
        : base(message)
    {
        StatusCode = statusCode;
    }
}
