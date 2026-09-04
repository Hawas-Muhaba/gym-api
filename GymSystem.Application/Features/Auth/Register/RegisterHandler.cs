using GymSystem.Application.Common.Interfaces;
using GymSystem.Domain.Entities;
using StaffEntity = GymSystem.Domain.Entities.Staff;
using MediatR;

namespace GymSystem.Application.Features.Auth.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand, string>
{
    private readonly IIdentityService _identityService;
    private readonly IApplicationDbContext _context;

    public RegisterHandler(IIdentityService identityService, IApplicationDbContext context)
    {
        _identityService = identityService;
        _context = context;
    }

    public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var (succeeded, userId, errors) = await _identityService.RegisterAsync(request.Email, request.Password, request.Role);
        if (!succeeded)
            throw new InvalidOperationException(string.Join(", ", errors));

        // Also create the matching business record, linked via UserId — our Option A design from earlier
        if (request.Role == "Client")
            _context.Clients.Add(new Client { UserId = userId, FullName = request.FullName, Phone = request.Phone, Email = request.Email });
        else if (request.Role == "Staff")
            _context.Staffs.Add(new StaffEntity { UserId = userId, FullName = request.FullName, Phone = request.Phone });

        await _context.SaveChangesAsync(cancellationToken);
        return userId!;
    }
}