using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using quotely_dotnet_api.Contexts;
using quotely_dotnet_api.Dtos;
using quotely_dotnet_api.Entities;
using quotely_dotnet_api.Extensions;
using quotely_dotnet_api.Interfaces;

namespace quotely_dotnet_api.BackgroundJobs;

public class GetAllQuoteJob(
    IQuotableHttpService quotableHttpService,
    AppDbContext appDbContext,
    ILogger<GetAllQuoteJob> logger,
    IUtilityService utilityService
)
{
    private readonly AppDbContext _appDbContext =
        appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));

    private readonly ILogger<GetAllQuoteJob> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly IQuotableHttpService _quotableHttpService =
        quotableHttpService ?? throw new ArgumentNullException(nameof(quotableHttpService));

    private readonly IUtilityService _utilityService =
        utilityService ?? throw new ArgumentNullException(nameof(utilityService));

    private static readonly SemaphoreSlim Lock = new(1, 1);

    public async Task Invoke()
    {
        _logger.LogInformation(
            "Starting the GetAllQuoteJob at {GetAllQuoteJobStartTime}...",
            DateTime.Now.ToCustomLogDateFormat()
        );

        try
        {
            await Lock.WaitAsync();

            var currentPage = 1;
            var totalPages = 1; // Default value for initialization

            List<string> allTags = [];

            do
            {
                _logger.LogInformation("Waiting for five seconds to avoid API limit...");
                await Task.Delay(10000);

                // Fetch data from API for the current page
                var allQuoteResponseDto = await _quotableHttpService.GetAsync<QuoteDto>(
                    $"quotes?page={currentPage}&limit=150"
                );

                if (allQuoteResponseDto == null)
                {
                    const string msg =
                        "Quotable API returned a null response or failed to map data";
                    _logger.LogCritical("{Msg}", msg);
                    throw new Exception(msg);
                }

                // Convert a response object to JSON string
                var quotableJsonResponse = JsonConvert.SerializeObject(allQuoteResponseDto);

                // Log the JSON string
                _logger.LogInformation(
                    "Quotable Discover API Response: {@GetAllQuotableJsonResponse}",
                    quotableJsonResponse
                );

                // Save quotes to the database
                foreach (var quote in allQuoteResponseDto.Results)
                {
                    allTags.AddRange(quote.Tags);

                    var existingQuote = await _appDbContext
                        .Quotes.Where(x => x.Id == quote.Id)
                        .FirstOrDefaultAsync();

                    if (existingQuote == null)
                    {
                        var newQuote = new Quote()
                        {
                            Id = quote.Id,
                            Author = quote.Author,
                            Content = quote.Content,
                            Length = quote.Length,
                            Tags = quote.Tags,
                            AuthorSlug = quote.AuthorSlug,
                            DateAdded = quote.DateAdded,
                            DateModified = quote.DateModified
                        };
                        await _appDbContext.AddAsync(newQuote);
                    }
                    else
                    {
                        existingQuote.Author = quote.Author;
                        existingQuote.Content = quote.Content;
                        existingQuote.Length = quote.Length;
                        existingQuote.Tags = quote.Tags;
                        existingQuote.AuthorSlug = quote.AuthorSlug;
                        existingQuote.DateAdded = quote.DateAdded;
                        existingQuote.DateModified = quote.DateModified;
                        _appDbContext.Entry(existingQuote).State = EntityState.Modified;
                    }
                }

                var result = await _appDbContext.SaveChangesAsync();
                if (result <= 0)
                    _logger.LogCritical("No Quotes were added when GetAllQuoteJob ran");

                // Set totalPages from the first response
                if (currentPage == 1)
                {
                    totalPages = allQuoteResponseDto.TotalPages;
                }

                _logger.LogInformation(
                    "Processed page {CurrentPage} of {TotalPages}...",
                    currentPage,
                    totalPages
                );

                currentPage++; // Increment page for the next iteration
            } while (currentPage <= totalPages);

            await _utilityService.AddOrUpdateTags(allTags);

            _logger.LogInformation("GetAllQuoteJob Executed Successfully");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.LogError(e, "Something Went Wrong when Running GetAllQuoteJob");
            throw;
        }
        finally
        {
            Lock.Release();
            _logger.LogInformation(
                "Finished the GetAllQuoteJob at {GetAllQuoteJobEndTime}...",
                DateTime.Now.ToCustomLogDateFormat()
            );
        }
    }
}
