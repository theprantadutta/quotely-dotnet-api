using quotely_dotnet_api.Dtos;
using quotely_dotnet_api.Views;

namespace quotely_dotnet_api.Interfaces;

public interface IQuoteOfTheDayService
{
    Task<QuoteOfTheDayResponseDto> GetAllQuoteOfTheDay(int pageNumber, int pageSize, bool getAllRows);

    Task<QuoteOfTheDayWithQuote> GetTodayQuoteOfTheDay();
}