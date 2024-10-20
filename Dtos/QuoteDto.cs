using System.Text.Json.Serialization;

namespace quotely_dotnet_api.Dtos;

public class QuoteDto
{
    public int Count { get; set; }

    public int TotalCount { get; set; }

    public int Page { get; set; }

    public int TotalPages { get; set; }

    // public int LastItemIndex { get; set; }

    public List<SingleQuoteDto> Results { get; set; } = [];
}

public class SingleQuoteDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string[] Tags { get; set; } = [];

    public string AuthorSlug { get; set; } = null!;

    public int Length { get; set; }

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
