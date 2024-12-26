namespace quotely_dotnet_api.Views;

public class DailyInspirationWithQuote
{
    public int DailyInspirationId { get; set; }
    public DateTime QuoteDate { get; set; }
    public DateTime DailyInspirationDateAdded { get; set; }
    public DateTime DailyInspirationDateModified { get; set; }

    public string QuoteId { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string[] Tags { get; set; } = [];
    public string AuthorSlug { get; set; } = null!;
    public int Length { get; set; }
    public DateTime QuoteDateAdded { get; set; }
    public DateTime QuoteDateModified { get; set; }
}