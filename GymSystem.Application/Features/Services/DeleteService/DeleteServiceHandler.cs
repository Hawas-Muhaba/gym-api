using GymSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Application.Features.Services.DeleteService;

public class DeleteServiceHandler : IRequestHandler<DeleteServiceCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteServiceHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await _context.Services
            .FirstOrDefaultAsync(s => s.Id == request.ServiceId, cancellationToken)
            ?? throw new KeyNotFoundException("Service not found.");

        service.IsDeleted = true;
        service.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}