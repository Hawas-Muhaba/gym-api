using GymSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GymSystem.Application.Common.Interfaces;
using GymSystem.Infrastructure.BackgroundJobs;
using GymSystem.Infrastructure.ExternalServices;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace GymSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<GymDbContext>(options =>
            options.UseNpgsql(connectionString));
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<GymDbContext>());

        services.AddHttpClient<ISmsSender, TelegramSender>()
    .AddResilienceHandler("telegram-retry", builder =>
    {
        builder.AddRetry(new HttpRetryStrategyOptions
        {
            MaxRetryAttempts = 3,
            Delay = TimeSpan.FromSeconds(2),
            BackoffType = DelayBackoffType.Exponential, // 2s, then 4s, then 8s — spacing out retries
        });

        builder.AddTimeout(TimeSpan.FromSeconds(10)); // don't wait forever on a hung request
    });

        services.AddScoped<ReminderJob>();

        services.AddHangfire(config => config
            .UsePostgreSqlStorage(connectionString));
        services.AddHangfireServer();

        return services;
    }
}