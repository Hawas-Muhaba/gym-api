using GymSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Application.Features.Appointments.GetStaffSchedule;

public class GetStaffScheduleHandler : IRequestHandler<GetStaffScheduleQuery, List<ScheduleItemDto>>
{
    private readonly IApplicationDbContext _context;
    public GetStaffScheduleHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<ScheduleItemDto>> Handle(GetStaffScheduleQuery request, CancellationToken cancellationToken)
    {
        var dayStart = DateTime.SpecifyKind(request.Date.Date, DateTimeKind.Utc);
        var dayEnd = dayStart.AddDays(1);

        return await _context.Appointments
            .Where(a => a.StaffId == request.StaffId && a.StartTime >= dayStart && a.StartTime < dayEnd)
            .OrderBy(a => a.StartTime)
            .Select(a => new ScheduleItemDto(a.Id, a.Client.FullName, a.Service.Name, a.StartTime, a.Status.ToString(), a.Service.Price, a.Staff.CommissionRate))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}