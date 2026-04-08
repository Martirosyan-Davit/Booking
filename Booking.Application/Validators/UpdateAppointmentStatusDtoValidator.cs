using Booking.Application.DTOs.Appointment;
using Booking.Domain.Enums;
using FluentValidation;

namespace Booking.Application.Validators;

public class UpdateAppointmentStatusDtoValidator : AbstractValidator<UpdateAppointmentStatusDto>
{
    public UpdateAppointmentStatusDtoValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required.")
            .Must
            (s => 
                s == nameof(AppointmentStatus.Confirmed) || s == nameof(AppointmentStatus.Cancelled)
            )
            .WithMessage("Status must be Confirmed or Cancelled");
    }
}
