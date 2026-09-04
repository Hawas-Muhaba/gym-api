using MediatR;

namespace GymSystem.Application.Features.Memberships.CreateOrRenewMembership;

public record CreateOrRenewMembershipCommand(int ClientId, int DurationMonths) : IRequest<int>;