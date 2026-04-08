using Booking.Application.DTOs.Appointment;
using FluentValidation;

namespace Booking.Application.Validators;

public class CreateAppointmentDtoValidator : AbstractValidator<CreateAppointmentDto>
{
    public CreateAppointmentDtoValidator()
    {
        RuleFor(x => x.ClientName)
            .NotEmpty().WithMessage("ClientName is required.")
            .MaximumLength(200).WithMessage("ClientName must not exceed 200 characters.");

        RuleFor(x => x.StylistId)
            .GreaterThan(0).WithMessage("StylistId must be a positive integer.");

        RuleFor(x => x.ServiceId)
            .GreaterThan(0).WithMessage("ServiceId must be a positive integer.");

        RuleFor(x => x.StartsAt)
            .NotEmpty().WithMessage("StartsAt is required.");
    }
}
