using quotely_dotnet_api.Dtos;
using quotely_dotnet_api.Views;

namespace quotely_dotnet_api.Interfaces;

public interface IMotivationMondayService
{
    Task<MotivationMondayResponseDto> GetAllMotivationMonday(
        int pageNumber,
        int pageSize,
        bool getAllRows
    );

    Task<MotivationMondayWithQuote> GetTodayMotivationMonday();
}
