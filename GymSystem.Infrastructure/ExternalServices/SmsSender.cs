using GymSystem.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace GymSystem.Infrastructure.ExternalServices;

public class SmsSender : ISmsSender
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SmsSender> _logger;

    public SmsSender(HttpClient httpClient, ILogger<SmsSender> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task SendAsync(string phoneNumber, string message, CancellationToken cancellationToken)
    {
        // For the timeline: log instead of hitting a real paid SMS API by default.
        // Swap this block for a real Twilio/WhatsApp Business API call if you have credentials —
        // the ISmsSender interface means nothing else in the app needs to change.
        _logger.LogInformation("SMS to {Phone}: {Message}", phoneNumber, message);

        await Task.CompletedTask;

        // Real implementation would look like:
        // var response = await _httpClient.PostAsJsonAsync("https://api.twilio.com/...", payload, cancellationToken);
    }
}