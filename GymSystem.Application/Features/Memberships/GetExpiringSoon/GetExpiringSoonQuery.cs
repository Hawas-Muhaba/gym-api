using MediatR;

namespace GymSystem.Application.Features.Memberships.GetExpiringSoon;

public record GetExpiringSoonQuery(int WithinDays = 7) : IRequest<List<ExpiringMembershipDto>>;

public record ExpiringMembershipDto(int ClientId, string ClientName, string Phone, DateTime ExpiryDate, string? TelegramChatId);