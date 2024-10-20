using Hangfire.Storage;

namespace quotely_dotnet_api.Interfaces;

public interface IUtilityService
{
    Task AddOrUpdateTags(List<string> tagTitles);

    RecurringJobDto GetJob(string jobId);
}
