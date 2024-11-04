using Hangfire;
using quotely_dotnet_api.BackgroundJobs;

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
    }
}
