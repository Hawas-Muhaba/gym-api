using GymSystem.Application.Common.Interfaces;
using GymSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Application.Features.Payments.RecordPayment;

public class RecordPaymentHandler : IRequestHandler<RecordPaymentCommand, int>
{
    private readonly IApplicationDbContext _context;
    public RecordPaymentHandler(IApplicationDbContext context) => _context = context;

    public async Task<int> Handle(RecordPaymentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _context.Appointments
            .Include(a => a.Staff)
            .FirstOrDefaultAsync(a => a.Id == request.AppointmentId, cancellationToken)
            ?? throw new KeyNotFoundException("Appointment not found.");

        // The core business rule: commission = payment amount × that staff member's rate.
        // No manual calculation needed anywhere else in the system — it happens exactly once, here.
        var commission = request.Amount * appointment.Staff.CommissionRate;

        var payment = new Payment
        {
            AppointmentId = request.AppointmentId,
            Amount = request.Amount,
            StaffCommissionAmount = commission,
            PaidAt = DateTime.UtcNow
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync(cancellationToken);
        return payment.Id;
    }
}