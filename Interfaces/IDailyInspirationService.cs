using quotely_dotnet_api.Dtos;
using quotely_dotnet_api.Views;

namespace quotely_dotnet_api.Interfaces;

public interface IDailyInspirationService
{
    Task<DailyInspirationResponseDto> GetAllDailyInspiration(
        int pageNumber,
        int pageSize,
        bool getAllRows
    );

    Task<DailyInspirationWithQuote> GetTodayDailyInspiration();
}
