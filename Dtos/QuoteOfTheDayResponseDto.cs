using quotely_dotnet_api.Views;

namespace quotely_dotnet_api.Dtos;

public class QuoteOfTheDayResponseDto
{
    public List<QuoteOfTheDayWithQuote> QuoteOfTheDayWithQuotes { get; set; } = [];
    public PaginationDto Pagination { get; set; } = null!;
}