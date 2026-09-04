using MediatR;

namespace GymSystem.Application.Features.Memberships.GetMembershipByClientId;

public record GetMembershipByClientIdQuery(int ClientId) : IRequest<MembershipDto?>;

public record MembershipDto(int Id, DateTime StartDate, DateTime ExpiryDate, bool IsActive);