using MediatR;

namespace GymSystem.Application.Features.Services.GetServices;

public record GetServicesQuery : IRequest<List<ServiceDto>>;

public record ServiceDto(int Id, string Name, decimal Price, int DurationMinutes);