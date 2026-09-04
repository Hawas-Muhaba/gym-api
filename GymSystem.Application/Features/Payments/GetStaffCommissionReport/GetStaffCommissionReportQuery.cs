using MediatR;

namespace GymSystem.Application.Features.Payments.GetStaffCommissionReport;

public record GetStaffCommissionReportQuery(int StaffId, DateTime FromDate, DateTime ToDate)
    : IRequest<StaffCommissionReportDto>;

public record StaffCommissionReportDto(
    int StaffId, string StaffName, decimal TotalRevenue, decimal TotalCommission, int PaymentCount, List<PaymentTransactionDto> Transactions
);

public record PaymentTransactionDto(int PaymentId, int AppointmentId, string ClientName, string ServiceName, decimal Amount, decimal Commission, DateTime PaidAt);

