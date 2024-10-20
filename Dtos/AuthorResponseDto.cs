using quotely_dotnet_api.Entities;

namespace quotely_dotnet_api.Dtos;

public class AuthorResponseDto
{
    public List<Author> Authors { get; set; } = [];
    public PaginationDto Pagination { get; set; } = null!;
}
