using System.Net;

namespace Booking.Application.Common.Exceptions;

/// <summary>
/// Request validation exception.
/// Contains detailed validation errors by field.
/// </summary>
public class ValidationException : BaseRequestException
{
    /// <summary>
    /// Validation errors grouped by property name.
    /// </summary>
    public Dictionary<string, string[]> Errors { get; }

    public ValidationException(Dictionary<string, string[]> errors)
        : base("Validation failed", HttpStatusCode.UnprocessableEntity)
    {
        Errors = errors;
    }

    public ValidationException(string propertyName, string errorMessage)
        : base("Validation failed", HttpStatusCode.UnprocessableEntity)
    {
        Errors = new Dictionary<string, string[]>
        {
            { propertyName, [errorMessage] }
        };
    }
}
