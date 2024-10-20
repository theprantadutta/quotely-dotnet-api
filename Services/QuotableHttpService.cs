using System.Net;
using quotely_dotnet_api.Interfaces;

namespace quotely_dotnet_api.Services;

public class QuotableHttpService(
    ILogger<QuotableHttpService> logger
) : IQuotableHttpService
{
    private readonly ILogger<QuotableHttpService> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<TReturn?> GetAsync<TReturn>(string relativeUri)
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("http://api.quotable.io");

        _logger.LogInformation("Sending New TMDB HTTP Request...");
        var res = await client.GetAsync(relativeUri);
        if (res.IsSuccessStatusCode)
        {
            _logger.LogInformation("Successful Status Code Received From Quotable");
            if (res.StatusCode == HttpStatusCode.NoContent)
            {
                _logger.LogInformation("No Content Received, Returning null from Quotable");
                return default;
            }

            if (res.IsSuccessStatusCode)
            {
                _logger.LogInformation("Sending Back JSON Response From Quotable");
                return await res.Content.ReadFromJsonAsync<TReturn>();
            }
        }

        _logger.LogCritical("UnSuccessful Status Code Received From Quotable");
        var msg = await res.Content.ReadAsStringAsync();
        _logger.LogError("{Msg}", msg);
        throw new Exception(msg);
    }
}