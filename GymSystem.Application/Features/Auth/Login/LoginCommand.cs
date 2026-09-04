using GymSystem.Application.Common.Interfaces;
using MediatR;

namespace GymSystem.Application.Features.Auth.Login;

public record LoginCommand(string Email, string Password) : IRequest<TokenResult>;