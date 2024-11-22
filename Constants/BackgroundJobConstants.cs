using Hangfire;

namespace quotely_dotnet_api.Constants;

public static class BackgroundJobConstants
{
    public static RecurringJobOptions JobOptions = new() { TimeZone = TimeZoneInfo.Local };
}
