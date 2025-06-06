﻿using Microsoft.EntityFrameworkCore;
using quotely_dotnet_api.Contexts;
using quotely_dotnet_api.Entities;
using quotely_dotnet_api.Extensions;
using quotely_dotnet_api.Interfaces;

namespace quotely_dotnet_api.BackgroundJobs.Notifications;

public class MotivationMondayJob(
    AppDbContext appDbContext,
    ILogger<MotivationMondayJob> logger,
    IFirebaseMessagingClient firebaseMessagingClient)
{
    private readonly AppDbContext _appDbContext =
        appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));

    private readonly ILogger<MotivationMondayJob> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly IFirebaseMessagingClient _firebaseMessagingClient =
        firebaseMessagingClient ?? throw new ArgumentNullException(nameof(firebaseMessagingClient));
    
    public async Task Invoke()
    {
        _logger.LogInformation(
            "Starting the MotivationMondayJob at {MotivationMondayJobStartTime}...",
            DateTime.Now.ToCustomLogDateFormat()
        );
        try
        {
            // Check if a quote for today has already been generated
            var today = DateTime.UtcNow.Date;
            var todayQuote = await _appDbContext.MotivationMondays
                .Where(x => x.QuoteDate.Date == today)
                .FirstOrDefaultAsync();

            if (todayQuote != null)
            {
                _logger.LogInformation("A motivation monday quote for today already exists with ID {QuoteId}", todayQuote.QuoteId);
                return; // Exit if a quote has already been generated for today
            }

            // Get a random quote from the database that has not been used as the quote of the day
            Quote? randomQuote;
            MotivationMonday? existingMotivationMonday;

            do
            {
                randomQuote = await _appDbContext.Quotes
                    .OrderBy(_ => Guid.NewGuid())
                    .FirstOrDefaultAsync();

                if (randomQuote == null)
                {
                    _logger.LogError("No random quote found in the database.");
                    throw new Exception("No Random Quote Found, which is weird");
                }

                // Check if this quote is already in the quote of the day table
                existingMotivationMonday = await _appDbContext.MotivationMondays
                    // ReSharper disable once AccessToModifiedClosure
                    .Where(x => x.QuoteId == randomQuote.Id)
                    .FirstOrDefaultAsync();

                if (existingMotivationMonday != null)
                {
                    _logger.LogInformation(
                        "Randomly selected quote with ID {QuoteId} has already been used as Quote of the Day. Retrying...",
                        randomQuote.Id
                    );
                }

            } while (existingMotivationMonday != null);

            // Now, randomQuote contains a quote that is not in the quote of the day
            await _appDbContext.MotivationMondays.AddAsync(new MotivationMonday
            {
                QuoteId = randomQuote.Id,
                QuoteDate = DateTime.UtcNow,
                DateAdded = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
            });

            await _appDbContext.SaveChangesAsync();

            await _firebaseMessagingClient.SendNotification(
                "motivation-monday",
                "Monday Motivation",
                $"{randomQuote.Content} - '{randomQuote.Author}'", 
                new Dictionary<string, string>()
                );
                
            _logger.LogInformation("MotivationMondayJob executed successfully with new Quote ID {QuoteId}", randomQuote.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while executing MotivationMondayJob");
            throw;
        }
        finally
        {
            _logger.LogInformation(
                "Finished the MotivationMondayJob at {MotivationMondayJobEndTime}...",
                DateTime.Now.ToCustomLogDateFormat()
            );
        }
    }
}
