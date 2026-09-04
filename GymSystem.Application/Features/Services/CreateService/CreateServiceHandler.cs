using GymSystem.Application.Common.Interfaces;
using GymSystem.Domain.Entities;
using MediatR;


namespace GymSystem.Application.Features.Services.CreateService;

public class CreateServiceHandler : IRequestHandler<CreateServiceCommand, int>
{
    private readonly IApplicationDbContext _context;
    public CreateServiceHandler(IApplicationDbContext context) => _context = context;

    public async Task<int> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = new Service { Name = request.Name, Price = request.Price, DurationMinutes = request.DurationMinutes };
        _context.Services.Add(service);
        await _context.SaveChangesAsync(cancellationToken);
        return service.Id;
    }
}