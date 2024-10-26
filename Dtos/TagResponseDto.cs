using quotely_dotnet_api.Entities;

namespace quotely_dotnet_api.Dtos;

public class TagResponseDto
{
    public List<Tag> Tags { get; set; } = [];
    public PaginationDto Pagination { get; set; } = null!;
}