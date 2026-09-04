using GymSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Application.Features.Appointments.GetAppointmentById;

public class GetAppointmentByIdHandler : IRequestHandler<GetAppointmentByIdQuery, AppointmentDetailDto>
{
    private readonly IApplicationDbContext _context;
    public GetAppointmentByIdHandler(IApplicationDbContext context) => _context = context;

    public async Task<AppointmentDetailDto> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Appointments
            .Where(a => a.Id == request.AppointmentId)
            .Select(a => new AppointmentDetailDto(
                a.Id, a.Client.FullName, a.Staff.FullName, a.Service.Name,
                a.StartTime, a.EndTime, a.Status.ToString()))
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new KeyNotFoundException("Appointment not found.");
    }
}