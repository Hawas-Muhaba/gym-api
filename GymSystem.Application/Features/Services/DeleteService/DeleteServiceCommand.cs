using MediatR;

namespace GymSystem.Application.Features.Services.DeleteService;

public record DeleteServiceCommand(int ServiceId) : IRequest;