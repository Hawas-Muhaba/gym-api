using GymSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Application.Features.Appointments.GetClientAppointments;

public class GetClientAppointmentsHandler
    : IRequestHandler<GetClientAppointmentsQuery, List<AppointmentDto>>
{
    private readonly IApplicationDbContext _context;

    public GetClientAppointmentsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<AppointmentDto>> Handle(
        GetClientAppointmentsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Appointments
            .Where(a => a.ClientId == request.ClientId)
            .OrderByDescending(a => a.StartTime)
            .Select(a => new AppointmentDto(
                a.Id,
                a.Staff.FullName,
                a.Service.Name,
                a.StartTime,
                a.Status.ToString()
            ))
            .AsNoTracking() // read-only — remember Lesson 4? no change-tracking overhead needed
            .ToListAsync(cancellationToken);
    }
}