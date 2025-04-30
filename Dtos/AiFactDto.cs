namespace quotely_dotnet_api.Dtos;

public class AiFactDto
{
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    public string AiFactCategory { get; set; } = null!;

    public string Provider { get; set; } = null!;

    public DateTime DateAdded { get; set; }

    public DateTime DateModified { get; set; }
}
