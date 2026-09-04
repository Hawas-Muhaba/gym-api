using MediatR;

namespace GymSystem.Application.Features.Clients.GetClients;

public record GetClientsQuery(int PageNumber = 1, int PageSize = 20, string? Search = null)
    : IRequest<PagedResult<ClientListDto>>;

public record ClientListDto(int Id, string FullName, string Phone, bool IsMembershipActive);

public record PagedResult<T>(List<T> Items, int TotalCount, int PageNumber, int PageSize);