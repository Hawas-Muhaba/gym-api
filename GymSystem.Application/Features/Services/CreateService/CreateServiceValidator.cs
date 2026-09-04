using FluentValidation;
namespace GymSystem.Application.Features.Services.CreateService;

public class CreateServiceValidator : AbstractValidator<CreateServiceCommand>
{
    public CreateServiceValidator()
    {
        RuleFor(x=>x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x=>x.Price).GreaterThan(0);
        RuleFor(x=>x.DurationMinutes).GreaterThan(0);
    }
}

