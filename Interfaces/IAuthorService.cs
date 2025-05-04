using quotely_dotnet_api.Dtos;
using quotely_dotnet_api.Entities;

namespace quotely_dotnet_api.Interfaces;

public interface IAuthorService
{
    Task<AuthorResponseDto> GetAllAuthors(int pageNumber, 
        int pageSize, 
        bool getAllRows,
        string? search);

    Task<Author?> GetAuthorDetails(string authorSlug);
}
