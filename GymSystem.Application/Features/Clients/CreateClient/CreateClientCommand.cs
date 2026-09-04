using MediatR;

namespace GymSystem.Application.Features.Clients.CreateClient;

public record CreateClientCommand(string FullName, string Phone, string Email, string? Notes) : IRequest<int>;