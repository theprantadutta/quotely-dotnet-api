namespace quotely_dotnet_api.Environment;

public class AiApiKeysConfiguration
{
    [ConfigurationKeyName("OPEN_AI_API_KEY")]
    public string OpenAiApiKey { get; set; } = null!;

    [ConfigurationKeyName("GEMINI_API_KEY")]
    public string GeminiApiKey { get; set; } = null!;

    [ConfigurationKeyName("DEEPSEEK_API_KEY")]
    public string DeepSeekApiKey { get; set; } = null!;

    [ConfigurationKeyName("MISTRAL_API_KEY")]
    public string MistralApiKey { get; set; } = null!;

    [ConfigurationKeyName("COHERE_API_KEY")]
    public string CohereApiKey { get; set; } = null!;
}
