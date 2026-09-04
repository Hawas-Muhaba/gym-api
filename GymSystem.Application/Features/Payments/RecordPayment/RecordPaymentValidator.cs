using FluentValidation;

namespace GymSystem.Application.Features.Payments.RecordPayment;

public class RecordPaymentValidator : AbstractValidator<RecordPaymentCommand>
{
    public RecordPaymentValidator()
    {
        RuleFor(x => x.AppointmentId).GreaterThan(0);
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}