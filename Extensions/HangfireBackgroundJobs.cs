using Hangfire;
using quotely_dotnet_api.BackgroundJobs;
using quotely_dotnet_api.BackgroundJobs.Notifications;
using quotely_dotnet_api.Entities;

namespace quotely_dotnet_api.Extensions;

public static class HangfireBackgroundJobs
{
    // This will return all Hangfire Background Jobs
    public static void AddHangfireBackgroundJobs(this IServiceCollection services)
    {
        // services.AddScoped<LoggingRecurringJob>();

        services.AddScoped<GetAllQuoteJob>();
        services.AddScoped<GetAllAuthorJob>();
        
        services.AddScoped<GetQuoteOfTheDayJob>();
        services.AddScoped<DailyInspirationJob>();
        services.AddScoped<MotivationMondayJob>();
    }

    // This will invoke all background jobs
    public static void UseHangfireBackgroundJobs(this WebApplication app)
    {
        // Schedule recurring jobs with names
        app.Services.GetRequiredService<IRecurringJobManager>()
            .AddOrUpdate<GetAllQuoteJob>("GetAllQuoteJob", job => job.Invoke(), Cron.Monthly);

        app.Services.GetRequiredService<IRecurringJobManager>()
            .AddOrUpdate<GetAllAuthorJob>("GetAllAuthorJob", job => job.Invoke(), Cron.Monthly);

        app.Services.GetRequiredService<IRecurringJobManager>()
            .AddOrUpdate<GetQuoteOfTheDayJob>("GetQuoteOfTheDayJob", job => job.Invoke(), Cron.Daily);

        app.Services.GetRequiredService<IRecurringJobManager>()
            .AddOrUpdate<DailyInspirationJob>("DailyInspirationJob", job => job.Invoke(), "0 11 * * 1");

        app.Services.GetRequiredService<IRecurringJobManager>()
            .AddOrUpdate<MotivationMondayJob>("MotivationMondayJob", job => job.Invoke(), "0 10 * * 1");
    }
}
