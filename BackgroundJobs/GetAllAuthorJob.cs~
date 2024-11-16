using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using quotely_dotnet_api.Contexts;
using quotely_dotnet_api.Dtos;
using quotely_dotnet_api.Entities;
using quotely_dotnet_api.Extensions;
using quotely_dotnet_api.Interfaces;

namespace quotely_dotnet_api.BackgroundJobs;

public class GetAllAuthorJob(
    IQuotableHttpService quotableHttpService,
    AppDbContext appDbContext,
    ILogger<GetAllAuthorJob> logger
)
{
    private readonly AppDbContext _appDbContext =
        appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));

    private readonly ILogger<GetAllAuthorJob> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly IQuotableHttpService _quotableHttpService =
        quotableHttpService ?? throw new ArgumentNullException(nameof(quotableHttpService));

    public async Task Invoke()
    {
        _logger.LogInformation(
            "Starting the GetAllAuthorJob at {GetAllQuoteJobStartTime}...",
            DateTime.Now.ToCustomLogDateFormat()
        );

        try
        {
            var currentPage = 1;
            var totalPages = 1; // Default value for initialization

            do
            {
                _logger.LogInformation("Waiting for five seconds to avoid API limit...");
                await Task.Delay(10000);

                // Fetch data from API for the current page
                var allAuthorResponseDto = await _quotableHttpService.GetAsync<AuthorDto>(
                    $"authors?page={currentPage}&limit=150"
                );

                if (allAuthorResponseDto == null)
                {
                    const string msg =
                        "Quotable API returned a null response or failed to map data";
                    _logger.LogCritical("{Msg}", msg);
                    throw new Exception(msg);
                }

                // Convert a response object to JSON string
                var authorJsonResponse = JsonConvert.SerializeObject(allAuthorResponseDto);

                // Log the JSON string
                _logger.LogInformation(
                    "Quotable Discover API Response: {@DiscoverMovieJsonResponse}",
                    authorJsonResponse
                );

                // Save quotes to the database
                foreach (var authorDto in allAuthorResponseDto.Results)
                {
                    var existingAuthor = await _appDbContext
                        .Authors.Where(x => x.Id == authorDto.Id)
                        .FirstOrDefaultAsync();

                    if (existingAuthor == null)
                    {
                        var newAuthor = new Author()
                        {
                            Id = authorDto.Id,
                            Name = authorDto.Name,
                            Bio = authorDto.Bio,
                            Slug = authorDto.Slug,
                            Description = authorDto.Description,
                            Link = authorDto.Link,
                            QuoteCount = authorDto.QuoteCount,
                            DateAdded = authorDto.DateAdded,
                            DateModified = authorDto.DateModified
                        };
                        await _appDbContext.AddAsync(newAuthor);
                    }
                    else
                    {
                        existingAuthor.Name = authorDto.Name;
                        existingAuthor.Bio = authorDto.Bio;
                        existingAuthor.Slug = authorDto.Slug;
                        existingAuthor.Description = authorDto.Description;
                        existingAuthor.Link = authorDto.Link;
                        existingAuthor.QuoteCount = authorDto.QuoteCount;
                        existingAuthor.DateAdded = authorDto.DateAdded;
                        existingAuthor.DateModified = authorDto.DateModified;
                        _appDbContext.Entry(existingAuthor).State = EntityState.Modified;
                    }
                }

                var result = await _appDbContext.SaveChangesAsync();
                if (result <= 0)
                    _logger.LogCritical("No Authors were added when GetAllAuthorJob ran");

                // Set totalPages from the first response
                if (currentPage == 1)
                {
                    totalPages = allAuthorResponseDto.TotalPages;
                }

                _logger.LogInformation(
                    "Processed page {CurrentPage} of {TotalPages}...",
                    currentPage,
                    totalPages
                );

                currentPage++; // Increment page for the next iteration

                _logger.LogInformation("Waiting for five seconds to avoid API limit...");
                await Task.Delay(5000);
            } while (currentPage <= totalPages);

            _logger.LogInformation("GetAllAuthorJob Executed Successfully");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Something Went Wrong when Running GetAllAuthorJob");
            throw;
        }
        finally
        {
            _logger.LogInformation(
                "Finished the GetAllAuthorJob at {GetAllQuoteJobEndTime}...",
                DateTime.Now.ToCustomLogDateFormat()
            );
        }
    }
}
