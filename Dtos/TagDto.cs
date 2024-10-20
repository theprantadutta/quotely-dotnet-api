using System.Text.Json.Serialization;

namespace quotely_dotnet_api.Dtos;

public class TagDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public int QuoteCount { get; set; }

    private DateTime _dateAdded;
    public DateTime DateAdded
    {
        get => _dateAdded;
        set => _dateAdded = DateTime.SpecifyKind(value, DateTimeKind.Utc); // Convert to UTC
    }

    private DateTime _dateModified;
    public DateTime DateModified
    {
        get => _dateModified;
        set => _dateModified = DateTime.SpecifyKind(value, DateTimeKind.Utc); // Convert to UTC
    }
}
