using System.Text.Json.Serialization;

namespace quotely_dotnet_api.Dtos;

public class AuthorDto
{
    public int Count { get; set; }
    
    public int TotalCount { get; set; }
    
    public int Page { get; set; }
    
    public int TotalPages { get; set; }
    
    public int LastItemIndex { get; set; }

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
    
    public DateTime DateAdded { get; set; }
    
    public DateTime DateModified { get; set; }
}