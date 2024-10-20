using quotely_dotnet_api.Entities;

namespace quotely_dotnet_api.Dtos;

public class QuoteResponseDto
{
    public List<Quote> Quotes { get; set; } = [];
    public PaginationDto Pagination { get; set; } = null!;
}
