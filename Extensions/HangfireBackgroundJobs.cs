using Hangfire;

namespace quotely_dotnet_api.Extensions;

public static class HangfireBackgroundJobs
{
    // This will return all Hangfire Background Jobs
    public static void AddHangfireBackgroundJobs(this IServiceCollection services)
    {
        // services.AddScoped<LoggingRecurringJob>();

        // services.AddScoped<DiscoverMovieSyncJob>();
    }

    // This will invoke all background jobs
    public static void UseHangfireBackgroundJobs(this WebApplication app)
    {
        // app.Services
        //     .GetRequiredService<IRecurringJobManagerV2>()
        //     .AddOrUpdate<LoggingRecurringJob>("logging-job", job => job.Invoke(), "*/5 * * * * *");

        // app.Services
        //     .GetRequiredService<IRecurringJobManagerV2>()
        //     .AddOrUpdate<DiscoverMovieSyncJob>(
        //         BackgroundJobNames.DiscoverMovieSyncJob,
        //         job => job.Invoke(),
        //         "0 0 * * MON,THU"
        //     );

        // app.Services
        //     .GetRequiredService<IRecurringJobManagerV2>()
        //     .AddOrUpdate<NowPlayingMovieSyncJob>(
        //         BackgroundJobNames.NowPlayingSyncJob,
        //         job => job.Invoke(),
        //         "0 1 * * MON,THU"
        //     );
        //
        // app.Services
        //     .GetRequiredService<IRecurringJobManagerV2>()
        //     .AddOrUpdate<PopularMovieSyncJob>(
        //         BackgroundJobNames.PopularMovieSyncJob,
        //         job => job.Invoke(),
        //         "0 2 * * MON,THU"
        //     );
        //
        // app.Services
        //     .GetRequiredService<IRecurringJobManagerV2>()
        //     .AddOrUpdate<TopRatedMovieSyncJob>(
        //         BackgroundJobNames.TopRatedSyncJob,
        //         job => job.Invoke(),
        //         "0 3 * * MON,THU"
        //     );
        //
        // app.Services
        //     .GetRequiredService<IRecurringJobManagerV2>()
        //     .AddOrUpdate<UpcomingMovieSyncJob>(
        //         BackgroundJobNames.UpcomingMovieSyncJob,
        //         job => job.Invoke(),
        //         "0 4 * * MON,THU"
        //     );
        //
        // app.Services
        //     .GetRequiredService<IRecurringJobManagerV2>()
        //     .AddOrUpdate<GenreWiseMovieSyncJob>(
        //         BackgroundJobNames.GenreWiseSyncJob,
        //         job => job.Invoke(),
        //         "0 5 * * MON,THU"
        //     );

        // app.Services
        //     .GetRequiredService<IRecurringJobManagerV2>()
        //     .AddOrUpdate<RecommendedMovieSyncJob>(
        //         "Recommended Movie Sync",
        //         job => job.Invoke(),
        //         "0 5 * * MON,THU"
        //     );
    }
}