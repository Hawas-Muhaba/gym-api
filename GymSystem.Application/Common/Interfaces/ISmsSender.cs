namespace GymSystem.Application.Common.Interfaces;
public interface ISmsSender
{
    Task SendAsync(string recipientId, string message, CancellationToken cancellationToken);
}