using Hangfire;
using quotely_dotnet_api.BackgroundJobs;
using quotely_dotnet_api.BackgroundJobs.Notifications;
using quotely_dotnet_api.Constants;

namespace quotely_dotnet_api.Extensions;

public static class HangfireBackgroundJobs
{
    // This will return all Hangfire Background Jobs
    public static void AddHangfireBackgroundJobs(this IServiceCollection services)
    {
        services.AddScoped<GetAllQuoteJob>();
        services.AddScoped<GetAllAuthorJob>();
        services.AddScoped<SendRandomNotificationJob>();

        // Notifications System
        services.AddScoped<GetQuoteOfTheDayJob>();
        services.AddScoped<DailyInspirationJob>();
        services.AddScoped<MotivationMondayJob>();
        services.AddScoped<RandomQuoteJob>();
    }

    // This will invoke all background jobs
    public static void UseHangfireBackgroundJobs(this WebApplication app)
    {
        var defaultJobOptions = BackgroundJobConstants.JobOptions;

        // Schedule recurring jobs with names
        app.Services
            .GetRequiredService<IRecurringJobManager>()
            .AddOrUpdate<GetAllQuoteJob>(
                "GetAllQuoteJob",
                job => job.Invoke(),
                Cron.Monthly,
                defaultJobOptions
            );

        app.Services
            .GetRequiredService<IRecurringJobManager>()
            .AddOrUpdate<GetAllAuthorJob>(
                "GetAllAuthorJob",
                job => job.Invoke(),
                Cron.Monthly,
                defaultJobOptions
            );

        app.Services
            .GetRequiredService<IRecurringJobManager>()
            .AddOrUpdate<GetQuoteOfTheDayJob>(
                "GetQuoteOfTheDayJob",
                job => job.Invoke(),
                Cron.Daily,
                defaultJobOptions
            );

        app.Services
            .GetRequiredService<IRecurringJobManager>()
            .AddOrUpdate<DailyInspirationJob>(
                "DailyInspirationJob",
                job => job.Invoke(),
                "0 11 * * *",
                defaultJobOptions
            );

        app.Services
            .GetRequiredService<IRecurringJobManager>()
            .AddOrUpdate<MotivationMondayJob>(
                "MotivationMondayJob",
                job => job.Invoke(),
                "0 10 * * 1",
                defaultJobOptions
            );

        app.Services
            .GetRequiredService<IRecurringJobManager>()
            .AddOrUpdate<SendRandomNotificationJob>(
                "SendRandomNotificationJob",
                job => job.Invoke(),
                Cron.Weekly,
                defaultJobOptions
            );
    }
}
