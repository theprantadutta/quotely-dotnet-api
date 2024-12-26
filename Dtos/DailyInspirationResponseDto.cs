using quotely_dotnet_api.Views;

namespace quotely_dotnet_api.Dtos;

public class DailyInspirationResponseDto
{
    public List<DailyInspirationWithQuote> DailyInspirationWithQuotes { get; set; } = [];
    public PaginationDto Pagination { get; set; } = null!;
}