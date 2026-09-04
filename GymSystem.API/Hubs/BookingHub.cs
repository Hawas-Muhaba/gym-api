using Microsoft.AspNetCore.SignalR;

namespace GymSystem.Api.Hubs;

public class BookingHub : Hub
{
    // Clients call this to join a "room" for their assigned staff member's updates —
    // so a Staff member only gets notified about their own bookings, not everyone's.
    public async Task JoinStaffGroup(int staffId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"staff-{staffId}");
    }
}