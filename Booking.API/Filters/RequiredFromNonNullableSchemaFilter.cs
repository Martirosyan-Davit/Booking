using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Booking.API.Filters;

/// <summary>
/// Populates the required array in OpenAPI schema based on nullable reference types.
/// Non-nullable properties are marked as required; nullable ones are optional.
/// </summary>
public sealed class RequiredFromNonNullableSchemaFilter : ISchemaFilter
{
    private static readonly JsonNamingPolicy CamelCase = JsonNamingPolicy.CamelCase;
    private readonly NullabilityInfoContext _nullabilityContext = new();

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties is null || schema.Properties.Count == 0)
            return;

        schema.Required.Clear();

        var type = context.Type;
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            var schemaKey = GetSchemaPropertyName(property);
            if (!schema.Properties.ContainsKey(schemaKey))
                continue;

            if (IsOptional(property))
                continue;

            schema.Required.Add(schemaKey);
        }
    }

    private static string GetSchemaPropertyName(PropertyInfo property)
    {
        var jsonName = property.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name;
        return jsonName ?? CamelCase.ConvertName(property.Name);
    }

    private bool IsOptional(PropertyInfo property)
    {
        var propType = property.PropertyType;

        if (Nullable.GetUnderlyingType(propType) is not null)
            return true;

        var nullability = _nullabilityContext.Create(property);
        return nullability.ReadState != NullabilityState.NotNull;
    }
}
