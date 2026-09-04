using MediatR;

namespace GymSystem.Application.Features.Auth.Register;

public record RegisterCommand(string Email, string Password, string FullName, string Phone, string Role) : IRequest<string>;