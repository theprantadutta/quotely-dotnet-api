using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using quotely_dotnet_api.Contexts;
using quotely_dotnet_api.Entities;
using quotely_dotnet_api.Environment;
using quotely_dotnet_api.Extensions;

namespace quotely_dotnet_api.BackgroundJobs;

public class GetAiFactJob(
    AppDbContext appDbContext,
    ILogger<GetAiFactJob> logger,
    AiApiKeysConfiguration apiKeys
)
{
    private readonly AppDbContext _appDbContext =
        appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));

    private readonly ILogger<GetAiFactJob> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly AiApiKeysConfiguration _apiKeys =
        apiKeys ?? throw new ArgumentNullException(nameof(apiKeys));

    // To Handle Open AI 1-Minute Rate Limit
    private static readonly SemaphoreSlim OpenAiRateLimiter = new(3, 3); // Allow 3 concurrent requests
    private static DateTime _lastOpenAiCall = DateTime.MinValue;
    private static readonly Lock OpenAiLock = new();

    private readonly List<string> _categories =
    [
        "Science",
        "History",
        "Space",
        "Technology",
        "Movies",
        "Music",
        "Animals",
        "Food",
        "Sports",
        "Art",
        "Literature",
        "Geography",
        "Mythology",
        "Inventions",
        "Plants",
        "Ocean",
        "Health",
        "Languages",
        "Gaming",
        "Weird Laws",
    ];

    private readonly List<string> _aiProviderList =
    [
        "Open AI",
        "Gemini",
        "DeepSeek",
        "Mistral",
        "Cohere",
    ];

    public async Task Invoke()
    {
        _logger.LogInformation(
            "Starting the GetAiFactJob at {GetAiFactJobStartTime}...",
            DateTime.Now.ToCustomLogDateFormat()
        );

        var successfulFacts = new List<AiFact>();

        try
        {
            foreach (var category in _categories)
            {
                var categoryIndex = _categories.IndexOf(category) + 1;
                var totalCategories = _categories.Count;

                _logger.LogInformation(
                    "[Progress] Processing category {CategoryIndex} of {TotalCategories}: {CategoryName}",
                    categoryIndex,
                    totalCategories,
                    category
                );

                foreach (var provider in _aiProviderList)
                {
                    var providerIndex = _aiProviderList.IndexOf(provider) + 1;
                    var totalProviders = _aiProviderList.Count;

                    _logger.LogInformation(
                        "[Progress] Processing provider {ProviderIndex} of {TotalProviders}: {ProviderName}",
                        providerIndex,
                        totalProviders,
                        provider
                    );

                    if (provider == "DeepSeek")
                    {
                        _logger.LogWarning(
                            "Skipping DeepSeek (provider {ProviderIndex} of {TotalProviders}) - API key not configured yet",
                            providerIndex,
                            totalProviders
                        );
                        continue;
                    }

                    await ProcessProviderForCategory(provider, category, successfulFacts);

                    // No delay after last provider
                    if (providerIndex >= totalProviders)
                        continue;
                    _logger.LogDebug("Waiting 1s before next provider...");
                    await Task.Delay(1000);
                }

                // No delay after last category
                if (categoryIndex >= totalCategories)
                    continue;
                _logger.LogDebug("Waiting 2s before next category...");
                await Task.Delay(2000);
            }

            // Bulk save all successful facts
            if (successfulFacts.Count != 0)
            {
                await _appDbContext.AiFacts.AddRangeAsync(successfulFacts);
                await _appDbContext.SaveChangesAsync();
                _logger.LogInformation("Saved {Count} facts to database", successfulFacts.Count);
            }
            else
            {
                _logger.LogWarning("No facts were collected to save");
            }

            _logger.LogInformation("GetAiFactJob Executed Successfully");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error saving facts to database");
        }
        finally
        {
            _logger.LogInformation(
                "Finished the GetAiFactJob at {GetAiFactJobEndTime}...",
                DateTime.Now.ToCustomLogDateFormat()
            );
        }
    }

    private async Task ProcessProviderForCategory(
        string provider,
        string category,
        List<AiFact> successfulFacts
    )
    {
        const int maxRetries = 3;
        var attempt = 0;
        var success = false;

        _logger.LogInformation(
            "Attempting to get fact from {Provider} about {Category}",
            provider,
            category
        );

        var now = DateTime.UtcNow;

        while (attempt < maxRetries && !success)
        {
            attempt++;
            try
            {
                string fact;
                switch (provider)
                {
                    case "Open AI":
                        fact = await GetWithRetries(() => GenerateOpenAiFact(category), attempt);
                        break;
                    case "Gemini":
                        fact = await GetWithRetries(() => GenerateGeminiFact(category), attempt);
                        break;
                    case "DeepSeek":
                        fact = await GetWithRetries(() => GenerateDeepSeekFact(category), attempt);
                        break;
                    case "Mistral":
                        fact = await GetWithRetries(() => GenerateMistralFact(category), attempt);
                        break;
                    case "Cohere":
                        fact = await GetWithRetries(() => GenerateCohereFact(category), attempt);
                        break;
                    default:
                        _logger.LogWarning("Unknown provider: {Provider}", provider);
                        return;
                }

                successfulFacts.Add(
                    new AiFact
                    {
                        Content = fact,
                        AiFactCategory = category,
                        Provider = provider,
                        DateAdded = now,
                        DateModified = now,
                    }
                );

                success = true;
                _logger.LogInformation(
                    "Successfully collected fact from {Provider} about {Category}",
                    provider,
                    category
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Attempt {Attempt} failed for {Provider} with category {Category}",
                    attempt,
                    provider,
                    category
                );
                await Task.Delay(1000 * attempt);
            }
        }

        if (!success)
        {
            _logger.LogError(
                "Could not get fact from {Provider} about {Category} after {MaxRetries} attempts",
                provider,
                category,
                maxRetries
            );
        }
    }

    private async Task<string> GetWithRetries(Func<Task<string>> factGenerator, int attempt)
    {
        try
        {
            return await factGenerator();
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Attempt {Attempt} failed: {ErrorMessage}", attempt, ex.Message);
            throw; // Re-throw to be handled by the outer retry loop
        }
    }

    private async Task<string> GenerateOpenAiFact(string category)
    {
        await OpenAiRateLimiter.WaitAsync();
        try
        {
            // Calculate minimum delay between requests (20 seconds between each of 3 requests = 1 minute)
            var minDelay = TimeSpan.FromSeconds(20);

            lock (OpenAiLock)
            {
                var timeSinceLastCall = DateTime.UtcNow - _lastOpenAiCall;
                if (timeSinceLastCall.TotalSeconds < 20)
                {
                    var delayMs = (20 - timeSinceLastCall.TotalSeconds) * 1000;
                    _logger.LogInformation("OpenAI rate limit: Delaying for {DelayMs}ms", delayMs);
                    Thread.Sleep((int)delayMs);
                }
                _lastOpenAiCall = DateTime.UtcNow;
            }

            var prompt =
                $"Give me one short, interesting, and true fun fact about {category}. Only respond with the fact, no introductions or explanations.";

            var requestBody = new
            {
                model = "gpt-4o-mini",
                messages = new[] { new { role = "user", content = prompt } },
            };

            var requestJson = System.Text.Json.JsonSerializer.Serialize(requestBody);
            var httpContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.openai.com");

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                _apiKeys.OpenAiApiKey
            );

            var response = await client.PostAsync("/v1/chat/completions", httpContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"OpenAI API request failed: {response.StatusCode}. Details: {errorContent}"
                );
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(responseJson);

            var fact = document
                .RootElement.GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            if (fact == null)
            {
                throw new Exception("OpenAI API response did not contain a fact.");
            }

            return fact.Trim();
        }
        finally
        {
            OpenAiRateLimiter.Release();
        }
    }

    private async Task<string> GenerateGeminiFact(string category)
    {
        var prompt =
            $"Give me one short, interesting, and true fun fact about {category}. Only respond with the fact, no introductions or explanations.";

        var requestBody = new
        {
            contents = new[] { new { parts = new[] { new { text = prompt } } } },
        };

        var requestJson = System.Text.Json.JsonSerializer.Serialize(requestBody);
        var httpContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

        using var client = new HttpClient();
        var apiUrl =
            $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_apiKeys.GeminiApiKey}";

        var response = await client.PostAsync(apiUrl, httpContent);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception(
                $"Gemini API request failed: {response.StatusCode}. Details: {errorContent}"
            );
        }

        var responseJson = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(responseJson);

        // Extract the fact from Gemini's response structure
        var fact = document
            .RootElement.GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        if (fact == null)
        {
            throw new Exception("Gemini API response did not contain a fact.");
        }

        return fact.Trim();
    }

    private async Task<string> GenerateDeepSeekFact(string category)
    {
        var prompt =
            $"Give me one short, interesting, and true fun fact about {category}. Only respond with the fact, no introductions or explanations.";

        var requestBody = new
        {
            model = "deepseek-chat",
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = "You are a factual assistant that responds concisely.",
                },
                new { role = "user", content = prompt },
            },
            stream = false,
        };

        var requestJson = System.Text.Json.JsonSerializer.Serialize(requestBody);
        var httpContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

        using var client = new HttpClient();
        client.BaseAddress = new Uri("https://api.deepseek.com");

        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            _apiKeys.DeepSeekApiKey
        );

        var response = await client.PostAsync("/chat/completions", httpContent);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception(
                $"DeepSeek API request failed: {response.StatusCode}. Details: {errorContent}"
            );
        }

        var responseJson = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(responseJson);

        // Extract the fact from DeepSeek's response (similar to OpenAI's structure)
        var fact = document
            .RootElement.GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        if (fact == null)
        {
            throw new Exception("DeepSeek API response did not contain a fact.");
        }

        return fact.Trim();
    }

    private async Task<string> GenerateMistralFact(string category)
    {
        var prompt =
            $"Give me one short, interesting, and true fun fact about {category}. Only respond with the fact, no introductions or explanations.";

        var requestBody = new
        {
            model = "mistral-large-latest",
            messages = new[] { new { role = "user", content = prompt } },
        };

        var requestJson = System.Text.Json.JsonSerializer.Serialize(requestBody);
        var httpContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

        using var client = new HttpClient();
        client.BaseAddress = new Uri("https://api.mistral.ai");

        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            _apiKeys.MistralApiKey
        );
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json")
        );

        var response = await client.PostAsync("/v1/chat/completions", httpContent);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception(
                $"Mistral API request failed: {response.StatusCode}. Details: {errorContent}"
            );
        }

        var responseJson = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(responseJson);

        // Mistral uses the same response structure as OpenAI
        var fact = document
            .RootElement.GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        if (fact == null)
        {
            throw new Exception("Mistral API response did not contain a fact.");
        }

        return fact.Trim();
    }

    private async Task<string> GenerateCohereFact(string category)
    {
        var prompt =
            $"Give me one short, interesting, and true fun fact about {category}. Only respond with the fact, no introductions or explanations.";

        var requestBody = new
        {
            model = "command-a-03-2025", // or "command" for the latest version
            messages = new[] { new { role = "user", content = prompt } },
            temperature = 0.7, // Optional parameter to control randomness
        };

        var requestJson = System.Text.Json.JsonSerializer.Serialize(requestBody);
        var httpContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

        using var client = new HttpClient();
        client.BaseAddress = new Uri("https://api.cohere.ai");

        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            _apiKeys.CohereApiKey
        );

        var response = await client.PostAsync("/compatibility/v1/chat/completions", httpContent);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception(
                $"Cohere API request failed: {response.StatusCode}. Details: {errorContent}"
            );
        }

        var responseJson = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(responseJson);

        var fact = document
            .RootElement.GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        if (string.IsNullOrWhiteSpace(fact))
        {
            throw new Exception("Cohere API returned an empty fact.");
        }

        return fact.Trim();
    }
}
