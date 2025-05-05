using quotely_dotnet_api.Environment;

namespace quotely_dotnet_api.Middlewares;

// This is the middleware responsible for handling API key Authentication
public class ApiKeyAuthMiddleware(ApiKeysConfiguration apiKeys) : IMiddleware
{
    private readonly ApiKeysConfiguration _apiKeys =
        apiKeys ?? throw new ArgumentNullException(nameof(apiKeys));

    // This method is responsible for actually implementing the logic of this middleware
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // Check if the request path is /health
        if (context.Request.Path.Equals("/health", StringComparison.OrdinalIgnoreCase))
        {
            // If the request path is /health, bypass the middleware and proceed to the next middleware in the pipeline
            await next(context);
            return;
        }

        // Proceed with the authentication logic for other endpoints
        if (!context.Request.Headers.TryGetValue("X-Api-Key", out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { errorMessage = "API Key Missing" });
            return;
        }

        var apiKey = _apiKeys.ApiKeyValue;
        if (!apiKey.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { errorMessage = "Invalid API Key" });
            return;
        }

        // If everything is fine, then go on to the controller
        await next(context);
    }
}
