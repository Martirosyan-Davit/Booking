using System.Reflection;
using Booking.Application.Common.Interfaces;
using AutoMapper;

namespace Booking.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        AllowNullCollections = true;
        ApplyMappingsFromAssembly(Assembly.GetAssembly(typeof(IAutoMap<,>))!);
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var types = assembly.GetExportedTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAutoMap<,>)))
            .ToList();

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);
            const string methodName = nameof(IAutoMap.CreateMap);
            var methodInfo = type.GetMethod(methodName)
                             ?? type.GetInterface("IAutoMap`2")?.GetMethod(methodName);
            methodInfo?.Invoke(instance, [this]);
        }
    }
}
