using Asp.Versioning;
using DotNetEnv;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using quotely_dotnet_api.Configurations;
using quotely_dotnet_api.Contexts;
using quotely_dotnet_api.Environment;
using quotely_dotnet_api.Extensions;
using quotely_dotnet_api.Interfaces;
using quotely_dotnet_api.Services;
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

// Configure Environment variables
builder
    .Configuration
    // only loads variables starting with QUOTELY__
    .AddEnvironmentVariables(s => s.Prefix = "QUOTELY__")
    .Build();

var pgsqlSection = builder.Configuration.GetSection("PGSQL");
builder.Services.Configure<PgsqlConfiguration>(pgsqlSection);

var apiKeySection = builder.Configuration.GetSection("AI");

// builder.Services.Configure<AiApiKeysConfiguration>(apiKeySection);
builder.Services.AddSingleton(
    apiKeySection.Get<AiApiKeysConfiguration>()
        ?? throw new Exception("All API Keys Should be there, you know")
);

// Add Postgres SQL Db Connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(pgsqlSection.GetValue<string>("CONNECTION_STRING"))
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
builder.Services.AddTransient<IQuotableHttpService, QuotableHttpService>();
builder.Services.AddTransient<IUtilityService, UtilityService>();
builder.Services.AddTransient<IQuoteService, QuoteService>();
builder.Services.AddTransient<IAuthorService, AuthorService>();
builder.Services.AddTransient<ITagService, TagService>();
builder.Services.AddTransient<IQuoteOfTheDayService, QuoteOfTheDayService>();
builder.Services.AddTransient<IDailyInspirationService, DailyInspirationService>();
builder.Services.AddTransient<IMotivationMondayService, MotivationMondayService>();
builder.Services.AddTransient<IApplicationService, ApplicationService>();
builder.Services.AddSingleton<IFirebaseMessagingClient, FirebaseMessagingClient>();

// Adding Hangfire Configuration
builder.Services.AddSingleton<HangfireAuthorizationConfiguration>();

builder.Services.AddControllers();

builder.Services.AddHealthChecks();

// Adding Hangfire
builder.Services.AddHangfire(config =>
{
    config
        .UsePostgreSqlStorage(options =>
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
        StatsPollingInterval = 30_000,
    }
);

app.Run();
