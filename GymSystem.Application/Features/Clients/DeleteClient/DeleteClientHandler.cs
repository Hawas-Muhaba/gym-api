using GymSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Application.Features.Clients.DeleteClient;

public class DeleteClientHandler : IRequestHandler<DeleteClientCommand>
{
    private readonly IApplicationDbContext _context;
    public DeleteClientHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(DeleteClientCommand request, CancellationToken cancellationToken)
    {
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == request.ClientId, cancellationToken)
            ?? throw new KeyNotFoundException("Client not found.");

        client.IsDeleted = true;  
        client.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}