using quotely_dotnet_api.Dtos;

namespace quotely_dotnet_api.Interfaces;

public interface ITagService
{
    Task<TagResponseDto> GetAllTags(int pageNumber, int pageSize, bool getAllRows);
}