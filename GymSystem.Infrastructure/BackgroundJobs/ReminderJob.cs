using GymSystem.Application.Common.Interfaces;
using GymSystem.Application.Features.Memberships.GetExpiringSoon;
using GymSystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GymSystem.Infrastructure.BackgroundJobs;

public class ReminderJob
{
    private readonly IMediator _mediator;
    private readonly ILogger<ReminderJob> _logger;
    private readonly ISmsSender _smsSender;
    private readonly IApplicationDbContext _context;

    public ReminderJob(IMediator mediator, ILogger<ReminderJob> logger, ISmsSender smsSender, IApplicationDbContext context)
    {
        _mediator = mediator;
        _logger = logger;
        _smsSender = smsSender;
        _context = context;
    }

    /// <summary>
    /// Sends renewal reminders to clients whose membership expires within 7 days.
    /// Scheduled via Hangfire (e.g. daily).
    /// </summary>
    public async Task SendMembershipReminders()
    {
        var expiringMemberships = await _mediator.Send(new GetExpiringSoonQuery(WithinDays: 7));

        foreach (var membership in expiringMemberships)
        {
            if (string.IsNullOrEmpty(membership.TelegramChatId)) continue; // skip clients who haven't connected yet

            var message = $"Hi {membership.ClientName}, your membership expires on {membership.ExpiryDate:MMM dd}. Renew now to keep your access!";
            await _smsSender.SendAsync(membership.TelegramChatId, message, CancellationToken.None);
        }
    }

    /// <summary>
    /// Sends appointment reminders to clients with upcoming appointments within the next 24 hours.
    /// Avoids duplicate sends by checking Notification.Sent.
    /// Scheduled via Hangfire (e.g. every hour).
    /// </summary>
    public async Task SendAppointmentReminders()
    {
        var now = DateTime.UtcNow;
        var window = now.AddHours(24); // remind clients up to 24 hours before their appointment

        // Load upcoming, non-cancelled appointments that haven't been reminded yet.
        var upcomingAppointments = await _context.Appointments
            .Include(a => a.Client)
            .Where(a => a.Status != BookingStatus.Cancelled
                        && a.StartTime >= now
                        && a.StartTime <= window
                        && !_context.Notifications.Any(n => n.AppointmentId == a.Id && n.Sent))
            .AsNoTracking()
            .ToListAsync(CancellationToken.None);

        foreach (var appointment in upcomingAppointments)
        {
            if (string.IsNullOrEmpty(appointment.Client.TelegramChatId))
            {
                _logger.LogInformation("Skipping appointment {AppointmentId}: client has no Telegram chat ID.", appointment.Id);
                continue;
            }

            var message = $"Hi {appointment.Client.FullName}, just a reminder: you have an appointment tomorrow at {appointment.StartTime:HH:mm}. See you soon!";

            try
            {
                await _smsSender.SendAsync(appointment.Client.TelegramChatId, message, CancellationToken.None);

                // Record that we sent this reminder so we don't send it again on the next job run.
                _context.Notifications.Add(new Domain.Entities.Notification
                {
                    AppointmentId = appointment.Id,
                    Message = message,
                    Sent = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to send appointment reminder for appointment {AppointmentId}.", appointment.Id);
                // Don't rethrow — one failed send shouldn't abort reminders for other clients.
            }
        }

        await _context.SaveChangesAsync(CancellationToken.None);
    }
}