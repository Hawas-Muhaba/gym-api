using GymSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Application.Features.Clients.UpdateClient;

public class UpdateClientHandler : IRequestHandler<UpdateClientCommand>
{
    private readonly IApplicationDbContext _context;
    public UpdateClientHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == request.ClientId, cancellationToken)
            ?? throw new KeyNotFoundException("Client not found.");

        client.FullName = request.FullName;
        client.Phone = request.Phone;
        client.Email = request.Email;
        client.Notes = request.Notes;

        await _context.SaveChangesAsync(cancellationToken);
    }
}