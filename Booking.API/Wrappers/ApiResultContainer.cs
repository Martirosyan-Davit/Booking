namespace Booking.API.Wrappers;

/// <summary>
/// Base API response container.
/// Contains common metadata that is returned in all server responses.
/// </summary>
public abstract class ApiResultContainerBase
{
    /// <summary>
    /// Metadata of the response (timestamp, CorrelationId and others).
    /// </summary>
    public MetaData Meta { get; init; } = new();
}

/// <summary>
/// Metadata of the API response.
/// Used for tracking requests and technical information.
/// </summary>
public class MetaData
{
    /// <summary>
    /// Timestamp of the response in UTC format.
    /// </summary>
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}

/// <summary>
/// Universal API success response container.
/// Contains useful data of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">
/// Type of the returned data.
/// </typeparam>
public class ApiResultContainer<T> : ApiResultContainerBase
{
    /// <summary>
    /// Data returned by the server.
    /// Can be null if the operation does not return an object.
    /// </summary>
    public required T Data { get; init; }
}

/// <summary>
/// API success response container without data.
/// Used for operations that do not return an entity (e.g. DELETE).
/// </summary>
public class ApiResultContainer: ApiResultContainerBase
{
}

/// <summary>
/// API failure response container.
/// Used for transferring error information to the client.
/// </summary>
public class ApiFailResultContainer : ApiResultContainerBase
{
    /// <summary>
    /// Human-readable error message.
    /// </summary>
    public required string ErrorMessage { get; init; }
    /// <summary>
    /// Application error code.
    /// Can be used by the client to handle different types of errors.
    /// </summary>
    public int ErrorCode { get; init; }
    
    /// <summary>
    /// Detailed validation errors by fields (only for 422).
    /// </summary>
    public Dictionary<string, string[]>? ValidationErrors { get; init; }
}