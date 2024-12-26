using Microsoft.EntityFrameworkCore;
using quotely_dotnet_api.Contexts;
using quotely_dotnet_api.Dtos;
using quotely_dotnet_api.Interfaces;
using quotely_dotnet_api.Views;

namespace quotely_dotnet_api.Services;

public class DailyInspirationService(AppDbContext appDbContext) : IDailyInspirationService
{
    private readonly AppDbContext _appDbContext =
        appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));

    public async Task<DailyInspirationResponseDto> GetAllDailyInspiration(
        int pageNumber,
        int pageSize,
        bool getAllRows
    )
    {
        if (getAllRows)
        {
            var allDailyInspirationWithQuotes = await _appDbContext.DailyInspirationWithQuotes
                .OrderBy(x => x.QuoteDate)
                .ToListAsync();
            return new DailyInspirationResponseDto()
            {
                DailyInspirationWithQuotes = allDailyInspirationWithQuotes,
                Pagination = new PaginationDto
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItemCount = allDailyInspirationWithQuotes.Count,
                }
            };
        }

        var totalItemCount = await _appDbContext.QuoteOfTheDayWithQuotes.CountAsync();

        var query = _appDbContext.DailyInspirationWithQuotes
            .OrderByDescending(x => x.QuoteDate)
            .AsSplitQuery()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        var allQuoteOfTheDay = await query.ToListAsync();

        return new DailyInspirationResponseDto()
        {
            DailyInspirationWithQuotes = allQuoteOfTheDay,
            Pagination = new PaginationDto()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItemCount = totalItemCount
            }
        };
    }

    public async Task<DailyInspirationWithQuote> GetTodayDailyInspiration()
    {
        var todaysInspiration = await _appDbContext.DailyInspirationWithQuotes
            .Where(x => x.QuoteDate.Date == DateTime.UtcNow.Date)
            .FirstOrDefaultAsync();

        if (todaysInspiration == null)
        {
            throw new Exception("No Daily Inspiration Found For Today");
        }

        return todaysInspiration;
    }
}
