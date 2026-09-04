using FluentValidation;

namespace GymSystem.Application.Features.Memberships.CreateOrRenewMembership;

public class CreateOrRenewMembershipValidator : AbstractValidator<CreateOrRenewMembershipCommand>
{
    public CreateOrRenewMembershipValidator()
    {
        RuleFor(x => x.ClientId).GreaterThan(0);
        RuleFor(x => x.DurationMonths).InclusiveBetween(1, 24);
    }
}