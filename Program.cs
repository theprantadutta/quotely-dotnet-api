using Asp.Versioning;
using DotNetEnv;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using quotely_dotnet_api.Configurations;
using quotely_dotnet_api.Contexts;
using quotely_dotnet_api.Environment;
using quotely_dotnet_api.Extensions;
using Serilog;
using Serilog.Debugging;

var builder = WebApplication.CreateBuilder(args);

// Show any errors with Serilog
SelfLog.Enable(Console.Error);

// Load .env files environment variables
Env.Load();

// Add services to the container.
// Add Serilog to the project
builder.Host.UseSerilog(
    (context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration)
);

// // Configure Tmdb section and bind it to TmdbConfiguration class
builder.Configuration
    // only loads variables starting with QUOTELY__
    .AddEnvironmentVariables(s => s.Prefix = "QUOTELY__")
    .Build();

// var tmdbSection = builder.Configuration.GetSection("TMDB");
// var omdbSection = builder.Configuration.GetSection("OMDB");
var pgsqlSection = builder.Configuration.GetSection("PGSQL");
//
// builder.Services.Configure<TmdbConfiguration>(tmdbSection);
// builder.Services.Configure<OmdbConfiguration>(omdbSection);
builder.Services.Configure<PgsqlConfiguration>(pgsqlSection);

// // Add Postgres SQL Db Connection
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseNpgsql(pgsqlSection.GetValue<string>("CONNECTION_STRING"))
);

// Adding API Versioning
builder.Services.AddApiVersioning(setupAction =>
{
    setupAction.AssumeDefaultVersionWhenUnspecified = true;
    setupAction.DefaultApiVersion = new ApiVersion(1, 0);
    setupAction.ReportApiVersions = true;
});

// Adding Memory Cache
builder.Services.AddMemoryCache();

// Adding HTTP Client
builder.Services.AddHttpClient();

// Adding auto mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Adding Services
// builder.Services.AddTransient<ITmdbHttpService, TmdbHttpService>();
// builder.Services.AddTransient<IOmdbHttpService, OmdbHttpService>();
// builder.Services.AddTransient<IMovieBackgroundJobService, MovieBackgroundJobService>();
// builder.Services.AddTransient<IImageService, ImageService>();
// builder.Services.AddTransient<IMovieService, MovieService>();
// builder.Services.AddTransient<IMovieProcessorService, MovieProcessorService>();

// Adding Hangfire Configuration
builder.Services.AddSingleton<HangfireAuthorizationConfiguration>();

builder.Services.AddControllers();

builder.Services.AddHealthChecks();

// Adding Hangfire
builder.Services.AddHangfire(config =>
{
    config
        .UsePostgreSqlStorage(
            options =>
                options.UseNpgsqlConnection(pgsqlSection.GetValue<string>("CONNECTION_STRING"))
        )
        .WithJobExpirationTimeout(TimeSpan.FromDays(10));
    config.UseFilter(new AutomaticRetryAttribute { Attempts = 0 });
});

builder.Services.AddHangfireServer(options =>
{
    options.SchedulePollingInterval = TimeSpan.FromSeconds(5);
    // options.WorkerCount = 10;
});

// Adding Recurring Jobs for Hangfire
builder.Services.AddHangfireBackgroundJobs();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSerilogRequestLogging();

// Mapping health checks url to '/health'
app.MapHealthChecks("/health");

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.UseHangfireBackgroundJobs();

app.UseHangfireDashboard(
    options: new DashboardOptions
    {
        Authorization = [HangfireAuthorizationConfiguration.GetBasicAuthFilter()],
        StatsPollingInterval = 30_000
    }
);

app.Run();