using quotely_dotnet_api.Dtos;
using quotely_dotnet_api.Entities;

namespace quotely_dotnet_api.Interfaces;

public interface IFactService
{
    Task<AiFactResponseDto> GetAiFacts(
        List<string> categories,
        List<string> aiProviders,
        bool getAllRows,
        int pageNumber,
        int pageSize
    );

    Task<List<string>> GetAiFactCategories();

    Task<List<string>> GetAiFactProviders();
}
