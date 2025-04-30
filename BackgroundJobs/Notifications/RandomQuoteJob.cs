using Microsoft.EntityFrameworkCore;
using quotely_dotnet_api.Contexts;
using quotely_dotnet_api.Entities;
using quotely_dotnet_api.Extensions;
using quotely_dotnet_api.Interfaces;

namespace quotely_dotnet_api.BackgroundJobs.Notifications;

public class RandomQuoteJob(
    AppDbContext appDbContext,
    ILogger<RandomQuoteJob> logger,
    IFirebaseMessagingClient firebaseMessagingClient
)
{
    private readonly AppDbContext _appDbContext =
        appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));

    private readonly ILogger<RandomQuoteJob> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly IFirebaseMessagingClient _firebaseMessagingClient =
        firebaseMessagingClient ?? throw new ArgumentNullException(nameof(firebaseMessagingClient));

    public async Task Invoke()
    {
        _logger.LogInformation(
            "Starting the RandomQuoteJob at {RandomQuoteJobStartTime}...",
            DateTime.Now.ToCustomLogDateFormat()
        );

        try
        {
            // Get a random quote from the database that has not been used as the quote of the day
            Quote? randomQuote;
            RandomQuote? existingRandomQuote;

            do
            {
                randomQuote = await _appDbContext
                    .Quotes.OrderBy(_ => Guid.NewGuid())
                    .FirstOrDefaultAsync();

                if (randomQuote == null)
                {
                    _logger.LogError("No random quote found in the database.");
                    throw new Exception("No Random Quote Found, which is weird");
                }

                // Check if this quote is already in the quote of the day table
                existingRandomQuote = await _appDbContext
                    .RandomQuotes
                    // ReSharper disable once AccessToModifiedClosure
                    .Where(x => x.QuoteId == randomQuote.Id)
                    .FirstOrDefaultAsync();

                if (existingRandomQuote != null)
                {
                    _logger.LogInformation(
                        "Randomly selected quote with ID {QuoteId} has already been used as A Random Quote before. Retrying...",
                        randomQuote.Id
                    );
                }
            } while (existingRandomQuote != null);

            // Now, randomQuote contains a quote not in the quote of the day
            await _appDbContext.RandomQuotes.AddAsync(
                new RandomQuote
                {
                    QuoteId = randomQuote.Id,
                    QuoteDate = DateTime.UtcNow,
                    DateAdded = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow,
                }
            );

            await _appDbContext.SaveChangesAsync();

            await _firebaseMessagingClient.SendNotification(
                "all",
                "From Quotely",
                $"{randomQuote.Content} - '{randomQuote.Author}'",
                new Dictionary<string, string>()
            );

            _logger.LogInformation(
                "RandomQuoteJob executed successfully with new Quote ID {QuoteId}",
                randomQuote.Id
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while executing RandomQuoteJob");
            throw;
        }
        finally
        {
            _logger.LogInformation(
                "Finished the RandomQuoteJob at {RandomQuoteJobEndTime}...",
                DateTime.Now.ToCustomLogDateFormat()
            );
        }
    }
}
