using FluentValidation;

namespace GymSystem.Application.Features.Appointments.CreateAppointment;

public class CreateAppointmentValidator : AbstractValidator<CreateAppointmentCommand>
{
    public CreateAppointmentValidator()
    {
        RuleFor(x => x.ClientId).GreaterThan(0);
        RuleFor(x => x.StaffId).GreaterThan(0);
        RuleFor(x => x.ServiceId).GreaterThan(0);
        RuleFor(x => x.StartTime).GreaterThan(DateTime.UtcNow.AddMinutes(-5)).WithMessage("Start time must be in the future.");
    }
}