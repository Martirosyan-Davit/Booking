using System.Net;
using System.Text.Json;
using Booking.API.Wrappers;
using Booking.Application.Common.Exceptions;
using ValidationException = Booking.Application.Common.Exceptions.ValidationException;

namespace Booking.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(
            exception,
            "Exception occurred: {Message}. InnerException: {InnerMessage}",
            exception.Message,
            exception.InnerException?.Message);

        var (statusCode, message, validationErrors) = GetErrorDetails(exception);

        var errorResult = new ApiFailResultContainer
        {
            ErrorMessage = message,
            ErrorCode = statusCode,
            ValidationErrors = validationErrors
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var json = JsonSerializer.Serialize(errorResult, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }

    private static (int statusCode, string message, Dictionary<string, string[]>? validationErrors) GetErrorDetails(
        Exception exception)
    {
        return exception switch
        {
            ValidationException vex => ((int)vex.StatusCode, vex.Message, vex.Errors),
            BaseRequestException ex => ((int)ex.StatusCode, ex.Message, null),
            _ => ((int)HttpStatusCode.InternalServerError, "An internal server error occurred. Please try again later.", null)
        };
    }
}

public static class ExceptionHandlingMiddlewareExtension
{
    public static void UseExceptionHandling(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
