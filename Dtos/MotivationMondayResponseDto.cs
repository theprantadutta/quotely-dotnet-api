using quotely_dotnet_api.Views;

namespace quotely_dotnet_api.Dtos;

public class MotivationMondayResponseDto
{
    public List<MotivationMondayWithQuote> MotivationMondayWithQuotes { get; set; } = [];
    public PaginationDto Pagination { get; set; } = null!;
}