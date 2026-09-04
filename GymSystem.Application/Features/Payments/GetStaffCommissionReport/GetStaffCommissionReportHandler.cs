using GymSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Application.Features.Payments.GetStaffCommissionReport;

public class GetStaffCommissionReportHandler : IRequestHandler<GetStaffCommissionReportQuery, StaffCommissionReportDto>
{
    private readonly IApplicationDbContext _context;
    public GetStaffCommissionReportHandler(IApplicationDbContext context) => _context = context;

    public async Task<StaffCommissionReportDto> Handle(GetStaffCommissionReportQuery request, CancellationToken cancellationToken)
    {
        var staff = await _context.Staffs
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == request.StaffId, cancellationToken)
            ?? throw new KeyNotFoundException($"Staff member with id {request.StaffId} was not found.");

        // Pull all payments for appointments handled by this staff member within the date range.
        // StaffCommissionAmount is already computed at payment-record time (Amount × CommissionRate),
        // so the report is just an aggregation — no re-calculation needed here.
        var fromDateUtc = DateTime.SpecifyKind(request.FromDate, DateTimeKind.Utc);
        var toDateUtc = DateTime.SpecifyKind(request.ToDate, DateTimeKind.Utc);

        var payments = await _context.Payments
            .AsNoTracking()
            .Where(p => p.Appointment.StaffId == request.StaffId
                        && p.PaidAt >= fromDateUtc
                        && p.PaidAt <= toDateUtc)
            .ToListAsync(cancellationToken);

        var totalRevenue    = payments.Sum(p => p.Amount);
        var totalCommission = payments.Sum(p => p.StaffCommissionAmount);

        var transactions = await _context.Payments
            .AsNoTracking()
            .Where(p => p.Appointment.StaffId == request.StaffId && p.PaidAt >= fromDateUtc && p.PaidAt <= toDateUtc)
            .OrderByDescending(p => p.PaidAt)
            .Select(p => new PaymentTransactionDto(p.Id, p.AppointmentId, p.Appointment.Client.FullName, p.Appointment.Service.Name, p.Amount, p.StaffCommissionAmount, p.PaidAt))
            .ToListAsync(cancellationToken);

        return new StaffCommissionReportDto(
            StaffId:         staff.Id,
            StaffName:       staff.FullName,
            TotalRevenue:    totalRevenue,
            TotalCommission: totalCommission,
            PaymentCount:    payments.Count,
            Transactions:    transactions
        );
    }
}
