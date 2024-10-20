using quotely_dotnet_api.Dtos;

namespace quotely_dotnet_api.Interfaces;

public interface IAuthorService
{
    Task<AuthorResponseDto> GetAllAuthors(int pageNumber, int pageSize, bool getAllRows);
}
