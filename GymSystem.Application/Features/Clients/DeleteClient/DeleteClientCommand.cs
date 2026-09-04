using MediatR;

namespace GymSystem.Application.Features.Clients.DeleteClient;

public record DeleteClientCommand(int ClientId) : IRequest;