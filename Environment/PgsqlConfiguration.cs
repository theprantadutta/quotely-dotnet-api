namespace quotely_dotnet_api.Environment;

public class PgsqlConfiguration
{
    [ConfigurationKeyName("CONNECTION_STRING")]
    public string ConnectionString { get; set; } = null!;
}