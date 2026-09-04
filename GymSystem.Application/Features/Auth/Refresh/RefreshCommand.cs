using MediatR;
using GymSystem.Application.Common.Interfaces;

namespace GymSystem.Application.Features.Auth.Refresh;

public record RefreshCommand(string AccessToken, string RefreshToken) : IRequest<TokenResult>;