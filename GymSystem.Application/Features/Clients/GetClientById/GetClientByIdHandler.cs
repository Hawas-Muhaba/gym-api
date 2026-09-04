using GymSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Application.Features.Clients.GetClientById;

public class GetClientByIdHandler : IRequestHandler<GetClientByIdQuery, ClientDetailDto>
{
    private readonly IApplicationDbContext _context;
    public GetClientByIdHandler(IApplicationDbContext context) => _context = context;

    public async Task<ClientDetailDto> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        var client = await _context.Clients
            .Include(c => c.Memberships)
            .Include(c => c.Appointments).ThenInclude(a => a.Service)
            .Include(c => c.Appointments).ThenInclude(a => a.Staff)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.ClientId, cancellationToken)
            ?? throw new KeyNotFoundException("Client not found.");

        var membership = client.Memberships
            .OrderByDescending(m => m.IsActive)
            .ThenByDescending(m => m.ExpiryDate)
            .FirstOrDefault();

        return new ClientDetailDto(
            client.Id, client.FullName, client.Phone, client.Email, client.Notes,
            membership?.IsActive ?? false, membership?.ExpiryDate,
            client.Appointments
                .OrderByDescending(a => a.StartTime)
                .Select(a => new AppointmentHistoryItemDto(a.StartTime, a.Service.Name, a.Staff.FullName, a.Status.ToString()))
                .ToList()
        );
    }
}