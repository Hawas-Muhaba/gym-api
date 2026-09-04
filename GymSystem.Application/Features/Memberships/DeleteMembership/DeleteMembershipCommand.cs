using MediatR;

namespace GymSystem.Application.Features.Memberships.DeleteMembership;

public record DeleteMembershipCommand(int MembershipId) : IRequest;