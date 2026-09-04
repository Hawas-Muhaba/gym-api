using MediatR;

namespace GymSystem.Application.Features.Clients.GetMyClientProfile;

public record GetMyClientProfileQuery : IRequest<MyClientProfileDto>;

public record MyClientProfileDto(int Id, string FullName, string Phone, string Email);