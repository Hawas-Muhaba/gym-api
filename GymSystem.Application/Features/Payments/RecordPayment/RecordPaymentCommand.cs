using MediatR;

namespace GymSystem.Application.Features.Payments.RecordPayment;

public record RecordPaymentCommand(int AppointmentId, decimal Amount) : IRequest<int>;