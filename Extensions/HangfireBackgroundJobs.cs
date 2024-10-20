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
    }

    // This will invoke all background jobs
    public static void UseHangfireBackgroundJobs(this WebApplication app)
    {
        // Schedule recurring jobs with names
        app.Services.GetRequiredService<IRecurringJobManager>()
            .AddOrUpdate<GetAllQuoteJob>("GetAllQuoteJob", job => job.Invoke(), Cron.Yearly);

        app.Services.GetRequiredService<IRecurringJobManager>()
            .AddOrUpdate<GetAllAuthorJob>("GetAllAuthorJob", job => job.Invoke(), Cron.Yearly);
    }
}
