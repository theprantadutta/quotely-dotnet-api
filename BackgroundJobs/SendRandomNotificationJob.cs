using Hangfire;
using quotely_dotnet_api.BackgroundJobs.Notifications;
using quotely_dotnet_api.Extensions;

namespace quotely_dotnet_api.BackgroundJobs;

public class SendRandomNotificationJob(ILogger<SendRandomNotificationJob> logger)
{
    private readonly ILogger<SendRandomNotificationJob> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    public Task Invoke()
    {
        _logger.LogInformation(
            "Starting the SendRandomNotificationJob at {SendRandomNotificationJobStartTime}...",
            DateTime.Now.ToCustomLogDateFormat()
        );

        try
        {
            var threeRandomDates =
                Generators.RandomDateTimeGenerator.GenerateThreeRandomDateTimes();

            foreach (var randomDate in threeRandomDates)
            {
                _logger.LogInformation(
                    "Scheduling a random quote background job to run at {ScheduledRandomNotificationJobStartTime}...",
                    randomDate.ToCustomLogDateFormat()
                );
                BackgroundJob.Schedule<RandomQuoteJob>(x => x.Invoke(), randomDate);
                _logger.LogInformation(
                    "Scheduled a random quote background job to run at {ScheduledRandomNotificationJobStartTime}",
                    randomDate.ToCustomLogDateFormat()
                );
            }

            _logger.LogInformation("SendRandomNotificationJob Executed Successfully");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.LogError(e, "Something Went Wrong when Running SendRandomNotificationJob");
            throw;
        }
        finally
        {
            _logger.LogInformation(
                "Finished the SendRandomNotificationJob at {SendRandomNotificationJobEndTime}...",
                DateTime.Now.ToCustomLogDateFormat()
            );
        }

        return Task.CompletedTask;
    }
}
