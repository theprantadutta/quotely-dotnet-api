using System.Text.Json.Serialization;

namespace quotely_dotnet_api.Dtos;

public class AuthorDto
{
    public int Count { get; set; }

    public int TotalCount { get; set; }

    public int Page { get; set; }

    public int TotalPages { get; set; }

    // public int LastItemIndex { get; set; }

    public List<SingleAuthorDto> Results { get; set; } = [];
}

public class SingleAuthorDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Bio { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Link { get; set; } = null!;

    public int QuoteCount { get; set; }

    public string Slug { get; set; } = null!;

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
