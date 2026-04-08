using Booking.API.Filters;
using Microsoft.OpenApi.Models;

namespace Booking.API.Configurations;

/// <summary>
/// Swagger / OpenAPI documentation configuration.
/// </summary>
public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SupportNonNullableReferenceTypes();

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "BeautyOS Appointment Service API",
                Version = "v1",
                Description = "Booking engine API for a beauty salon"
            });

            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            if (File.Exists(xmlPath))
                options.IncludeXmlComments(xmlPath);

            options.SchemaFilter<RequiredFromNonNullableSchemaFilter>();
        });

        return services;
    }

    public static void UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}
