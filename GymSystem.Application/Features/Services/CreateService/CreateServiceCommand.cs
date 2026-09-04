using MediatR;

namespace GymSystem.Application.Features.Services.CreateService;

public record CreateServiceCommand(string Name, decimal Price, int DurationMinutes) : IRequest<int>;