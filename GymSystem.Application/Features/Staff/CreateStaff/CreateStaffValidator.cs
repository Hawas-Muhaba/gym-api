using FluentValidation;

namespace GymSystem.Application.Features.Staff.CreateStaff;

public class CreateStaffValidator : AbstractValidator<CreateStaffCommand>
{
    public CreateStaffValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Phone).NotEmpty().MaximumLength(20);
        RuleFor(x => x.CommissionRate).InclusiveBetween(0, 1)
            .WithMessage("Commission rate must be between 0 and 1 (e.g. 0.20 = 20%).");
    }
}