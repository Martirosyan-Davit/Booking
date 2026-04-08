using System.Reflection;
using Booking.Application.Common.Mappings;
using Booking.Application.Interfaces.Services;
using Booking.Application.Services;
using Booking.Application.Services.Appointments;
using Booking.Application.Services.Stylists;
using Microsoft.Extensions.DependencyInjection;

namespace Booking.Application.Extensions;

public static class DependencyExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(new[] { Assembly.GetAssembly(typeof(MappingProfile)) }, ServiceLifetime.Scoped);

        services.AddScoped<IStylistService, StylistService>();
        services.AddScoped<IServiceService, ServiceService>();
        services.AddScoped<IAppointmentService, AppointmentService>();

        return services;
    }
}
