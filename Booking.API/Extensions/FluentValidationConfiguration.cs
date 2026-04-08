using Booking.API.Wrappers;
using Booking.Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Extensions;

/// <summary>
/// Centralized FluentValidation configuration for the API.
/// </summary>
public static class FluentValidationConfiguration
{
    /// <summary>
    /// Configures automatic FluentValidation with custom error response format.
    /// </summary>
    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssembly(typeof(CreateStylistDtoValidator).Assembly);

        services.Configure<ApiBehaviorOptions>(ConfigureValidationErrorResponse);

        return services;
    }

    /// <summary>
    /// Configures the response format for validation errors (422 Unprocessable Entity).
    /// Both model-binding errors and FluentValidation errors are returned consistently.
    /// </summary>
    private static void ConfigureValidationErrorResponse(ApiBehaviorOptions options)
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .ToDictionary(
                    e => e.Key,
                    e => e.Value!.Errors.Select(x => x.ErrorMessage).ToArray()
                );

            var result = new ApiFailResultContainer
            {
                ErrorMessage = "Validation failed",
                ErrorCode = 422,
                ValidationErrors = errors
            };

            return new UnprocessableEntityObjectResult(result);
        };
    }
}
