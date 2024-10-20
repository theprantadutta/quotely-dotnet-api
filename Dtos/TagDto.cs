using System.Text.Json.Serialization;

namespace quotely_dotnet_api.Dtos;

public class TagDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public int QuoteCount { get; set; }
    
    public DateTime DateAdded { get; set; }
    
    public DateTime DateModified { get; set; }
}