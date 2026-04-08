using Booking.Application.DTOs.Appointment;
using FluentValidation;

namespace Booking.Application.Validators;

public class GetAppointmentsRequestDtoValidator : AbstractValidator<GetAppointmentsRequestDto>
{
    public GetAppointmentsRequestDtoValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber must be at least 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");

        RuleFor(x => x.StylistId)
            .GreaterThan(0).WithMessage("StylistId must be a positive integer.")
            .When(x => x.StylistId.HasValue);

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status must be a valid value: Pending, Confirmed, or Cancelled.")
            .When(x => x.Status.HasValue);
    }
}
