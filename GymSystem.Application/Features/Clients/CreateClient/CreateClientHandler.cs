using GymSystem.Application.Common.Interfaces;
using GymSystem.Domain.Entities;
using MediatR;

namespace GymSystem.Application.Features.Clients.CreateClient;

public class CreateClientHandler : IRequestHandler<CreateClientCommand, int>
{
    private readonly IApplicationDbContext _context;
    public CreateClientHandler(IApplicationDbContext context) => _context = context;

    public async Task<int> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        var client = new Client
        {
            FullName = request.FullName,
            Phone = request.Phone,
            Email = request.Email,
            Notes = request.Notes
        };

        _context.Clients.Add(client);
        await _context.SaveChangesAsync(cancellationToken);
        return client.Id;
    }
}