using quotely_dotnet_api.Entities;

namespace quotely_dotnet_api.Dtos;

public class AiFactResponseDto
{
    public List<AiFact> AiFacts { get; set; } = [];
    public PaginationDto Pagination { get; set; } = null!;
}
