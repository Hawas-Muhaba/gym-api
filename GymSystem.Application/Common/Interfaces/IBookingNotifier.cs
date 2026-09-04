namespace GymSystem.Application.Common.Interfaces;

public interface IBookingNotifier
{
    Task NotifyNewBooking(int staffId, object bookingData, CancellationToken cancellationToken);
}