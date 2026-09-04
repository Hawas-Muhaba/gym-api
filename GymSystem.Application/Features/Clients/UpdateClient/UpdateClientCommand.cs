using MediatR;

namespace GymSystem.Application.Features.Clients.UpdateClient;

public record UpdateClientCommand(int ClientId, string FullName, string Phone, string Email, string? Notes) : IRequest;