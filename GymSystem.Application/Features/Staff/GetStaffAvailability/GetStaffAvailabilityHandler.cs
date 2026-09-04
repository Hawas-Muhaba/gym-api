using GymSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Application.Features.Staff.GetStaffAvailability;

public class GetStaffAvailabilityHandler : IRequestHandler<GetStaffAvailabilityQuery, List<TimeSlotDto>>
{
    private readonly IApplicationDbContext _context;
    public GetStaffAvailabilityHandler(IApplicationDbContext context) => _context = context;

    private static readonly TimeSpan WorkStart = TimeSpan.FromHours(9);
    private static readonly TimeSpan WorkEnd = TimeSpan.FromHours(18);
    private static readonly TimeSpan SlotLength = TimeSpan.FromMinutes(60);

    public async Task<List<TimeSlotDto>> Handle(GetStaffAvailabilityQuery request, CancellationToken cancellationToken)
    {
        var dayStart = DateTime.SpecifyKind(request.Date.Date, DateTimeKind.Utc);
        var dayEnd = dayStart.AddDays(1);

        var bookedTimes = await _context.Appointments
            .AsNoTracking()
            .Where(a => a.StaffId == request.StaffId
                     && a.StartTime >= dayStart
                     && a.StartTime < dayEnd
                     && a.Status != Domain.Enums.BookingStatus.Cancelled)
            .Select(a => a.StartTime)
            .ToListAsync(cancellationToken);

        var slots = new List<TimeSlotDto>();
        for (var time = dayStart.Add(WorkStart); time < dayStart.Add(WorkEnd); time += SlotLength)
        {
            bool isTaken = bookedTimes.Any(b => b == time);
            slots.Add(new TimeSlotDto(time, IsAvailable: !isTaken));
        }

        return slots;
    }
}