using MediatR;

namespace GymSystem.Application.Features.Payments.DeletePayment;

public record DeletePaymentCommand(int PaymentId) : IRequest;