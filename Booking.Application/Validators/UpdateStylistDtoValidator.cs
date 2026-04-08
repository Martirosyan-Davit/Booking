using Booking.Application.DTOs.Stylist;
using FluentValidation;

namespace Booking.Application.Validators;

public class UpdateStylistDtoValidator : AbstractValidator<UpdateStylistDto>
{
    public UpdateStylistDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Specialization)
            .NotEmpty().WithMessage("Specialization is required.")
            .MaximumLength(100).WithMessage("Specialization must not exceed 100 characters.");

        RuleFor(x => x.WorkStart)
            .NotEmpty().WithMessage("WorkStart is required.")
            .Matches(@"^([01]\d|2[0-3]):[0-5]\d$").WithMessage("WorkStart must be in HH:mm format.");

        RuleFor(x => x.WorkEnd)
            .NotEmpty().WithMessage("WorkEnd is required.")
            .Matches(@"^([01]\d|2[0-3]):[0-5]\d$").WithMessage("WorkEnd must be in HH:mm format.");
    }
}
