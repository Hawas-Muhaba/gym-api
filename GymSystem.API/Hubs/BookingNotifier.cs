using GymSystem.Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace GymSystem.Api.Hubs;

public class BookingNotifier : IBookingNotifier
{
    private readonly IHubContext<BookingHub> _hubContext;
    public BookingNotifier(IHubContext<BookingHub> hubContext) => _hubContext = hubContext;

    public async Task NotifyNewBooking(int staffId, object bookingData, CancellationToken cancellationToken)
    {
        await _hubContext.Clients.Group($"staff-{staffId}").SendAsync("NewBookingCreated", bookingData, cancellationToken);
    }
}