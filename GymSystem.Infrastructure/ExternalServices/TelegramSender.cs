using System.Net.Http.Json;
using GymSystem.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GymSystem.Infrastructure.ExternalServices;

public class TelegramSender : ISmsSender
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TelegramSender> _logger;
    private readonly string _botToken;

    public TelegramSender(HttpClient httpClient, IConfiguration configuration, ILogger<TelegramSender> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _botToken = configuration["Telegram:BotToken"]!;
    }

    public async Task SendAsync(string recipientId, string message, CancellationToken cancellationToken)
    {
        var url = $"https://api.telegram.org/bot{_botToken}/sendMessage";

        var payload = new { chat_id = recipientId, text = message };
        var response = await _httpClient.PostAsJsonAsync(url, payload, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogWarning("Telegram send failed for {RecipientId}: {Error}", recipientId, error);
            throw new HttpRequestException($"Telegram API error: {response.StatusCode}");
            // Throwing here matters — it's what lets Polly (next step) know to retry
        }

        _logger.LogInformation("Telegram message sent to {RecipientId}", recipientId);
    }
}