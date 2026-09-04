using System.Text;
using System.IdentityModel.Tokens.Jwt;
using GymSystem.Application;
using GymSystem.Application.Common.Interfaces;
using GymSystem.Api.Hubs;
using GymSystem.Api.Middleware;
using GymSystem.API.Services;
using GymSystem.Infrastructure;
using GymSystem.Infrastructure.BackgroundJobs;
using GymSystem.Infrastructure.Identity;
using GymSystem.Infrastructure.Persistence;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // claims now stay as short names like "role"
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IBookingNotifier, BookingNotifier>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 6; // relaxed for demo purposes; tighten for real production use
    })
    .AddEntityFrameworkStores<GymDbContext>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
            RoleClaimType = System.Security.Claims.ClaimTypes.Role
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/bookingHub"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

//lets Angular (a different origin) call this API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularApp", policy => policy
        .WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()); // needed for SignalR
});

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddOpenApi();
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddFixedWindowLimiter("booking", limiterOptions =>
    {
        limiterOptions.PermitLimit = 5;              // 5 booking attempts...
        limiterOptions.Window = TimeSpan.FromMinutes(1); // ...per minute
        limiterOptions.QueueLimit = 0;                // no queueing — excess requests rejected immediately
    });

    options.AddFixedWindowLimiter("auth", limiterOptions =>
    {
        limiterOptions.PermitLimit = 5;               // stricter — prevents brute-force password guessing
        limiterOptions.Window = TimeSpan.FromMinutes(5);
        limiterOptions.QueueLimit = 0;
    });
});
var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseCors("AngularApp");     // 👈 order matters: CORS before Auth
app.UseAuthentication();       
app.UseAuthorization();        
app.UseRateLimiter();
app.MapControllers();
app.MapHub<BookingHub>("/bookingHub"); 

try
{
    app.UseHangfireDashboard();
    RecurringJob.AddOrUpdate<ReminderJob>(
        "membership-reminders",
        job => job.SendMembershipReminders(),
        Cron.Daily(9));

    RecurringJob.AddOrUpdate<ReminderJob>(
        "appointment-reminders",
        job => job.SendAppointmentReminders(),
        Cron.Hourly());
}
catch (Exception ex)
{
    app.Logger.LogWarning(ex, "Could not initialize Hangfire recurring jobs. Ensure PostgreSQL database is running and migrated.");
}

app.Run();