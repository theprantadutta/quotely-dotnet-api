namespace quotely_dotnet_api.Views;

public class QuoteOfTheDayWithQuote
{
    public int QuoteOfTheDayId { get; set; }
    public DateTime QuoteDate { get; set; }
    public DateTime QuoteOfTheDayDateAdded { get; set; }
    public DateTime QuoteOfTheDayDateModified { get; set; }

    public string QuoteId { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string[] Tags { get; set; } = Array.Empty<string>();
    public string AuthorSlug { get; set; } = null!;
    public int Length { get; set; }
    public DateTime QuoteDateAdded { get; set; }
    public DateTime QuoteDateModified { get; set; }
}
