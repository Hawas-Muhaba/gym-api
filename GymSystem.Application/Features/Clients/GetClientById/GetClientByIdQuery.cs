using MediatR;

namespace GymSystem.Application.Features.Clients.GetClientById;

public record GetClientByIdQuery(int ClientId) : IRequest<ClientDetailDto>;

public record ClientDetailDto(
    int Id, string FullName, string Phone, string Email, string? Notes,
    bool IsMembershipActive, DateTime? MembershipExpiry,
    List<AppointmentHistoryItemDto> PastAppointments
);

public record AppointmentHistoryItemDto(DateTime Date, string ServiceName, string StaffName, string Status);