using Hangfire;
using Hangfire.Storage;
using Microsoft.EntityFrameworkCore;
using NUlid;
using quotely_dotnet_api.Contexts;
using quotely_dotnet_api.Entities;
using quotely_dotnet_api.Interfaces;
using Slugify;

namespace quotely_dotnet_api.Services;

public class UtilityService(AppDbContext appDbContext, ILogger<UtilityService> logger)
    : IUtilityService
{
    private readonly AppDbContext _appDbContext =
        appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));

    private readonly ILogger<UtilityService> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task AddOrUpdateTags(List<string> tagTitles)
    {
        foreach (var tag in tagTitles)
        {
            var existingTag = await _appDbContext
                .Tags.Where(x => x.Name == tag)
                .FirstOrDefaultAsync();

            if (existingTag == null)
            {
                var nanoId = Ulid.NewUlid().ToString();
                var newTag = new Tag()
                {
                    Id = nanoId,
                    Name = tag,
                    Slug = new SlugHelper().GenerateSlug(tag),
                    QuoteCount = await GetTotalQuoteCountForTag(tag),
                    DateAdded = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow
                };
                await _appDbContext.Tags.AddAsync(newTag);
            }
            else
            {
                existingTag.QuoteCount = await GetTotalQuoteCountForTag(existingTag.Name);
                existingTag.DateModified = DateTime.UtcNow;
                _appDbContext.Entry(existingTag).State = EntityState.Modified;
            }
        }

        var result = await _appDbContext.SaveChangesAsync();
        if (result <= 0)
            _logger.LogCritical("No Tags were added");

        _logger.LogInformation("{TotalAddedTags} tags were affected", result);
    }

    public RecurringJobDto GetJob(string jobId)
    {
        using var connection = JobStorage.Current.GetConnection();
        var currentJob = connection.GetRecurringJobs().FirstOrDefault(p => p.Id == jobId);
        if (currentJob != null)
            return currentJob;

        _logger.LogError("Failed to get the Job ID with {JobId}", jobId);
        throw new Exception($"Failed to get the Job with ID {jobId}");
    }

    private async Task<int> GetTotalQuoteCountForTag(string tag)
    {
        if (string.IsNullOrWhiteSpace(tag))
            throw new ArgumentException("Tag cannot be null or empty.", nameof(tag));

        // Use EF Core to count the number of quotes containing the specified tag
        var count = await _appDbContext
            .Quotes.Where(q => q.Tags.Any(t => string.Equals(t, tag)))
            .CountAsync();

        return count;
    }
}
