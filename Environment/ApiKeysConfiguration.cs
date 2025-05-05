namespace quotely_dotnet_api.Environment;

public class ApiKeysConfiguration
{
    [ConfigurationKeyName("API_KEY_VALUE")]
    public string ApiKeyValue { get; set; } = null!;
}
