using Microsoft.EntityFrameworkCore;
using quotely_dotnet_api.Contexts;
using quotely_dotnet_api.Dtos;
using quotely_dotnet_api.Interfaces;
using quotely_dotnet_api.Views;

namespace quotely_dotnet_api.Services;

public class MotivationMondayService(AppDbContext appDbContext) : IMotivationMondayService
{
    private readonly AppDbContext _appDbContext =
        appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));

    public async Task<MotivationMondayResponseDto> GetAllMotivationMonday(
        int pageNumber,
        int pageSize,
        bool getAllRows
    )
    {
        if (getAllRows)
        {
            var allMotivationMondayWithQuotes = await _appDbContext.MotivationMondayWithQuotes
                .OrderBy(x => x.QuoteDate)
                .ToListAsync();
            return new MotivationMondayResponseDto()
            {
                MotivationMondayWithQuotes = allMotivationMondayWithQuotes,
                Pagination = new PaginationDto
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItemCount = allMotivationMondayWithQuotes.Count,
                }
            };
        }

        var totalItemCount = await _appDbContext.QuoteOfTheDayWithQuotes.CountAsync();

        var query = _appDbContext.MotivationMondayWithQuotes
            .OrderByDescending(x => x.QuoteDate)
            .AsSplitQuery()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        var allQuoteOfTheDay = await query.ToListAsync();

        return new MotivationMondayResponseDto()
        {
            MotivationMondayWithQuotes = allQuoteOfTheDay,
            Pagination = new PaginationDto()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItemCount = totalItemCount
            }
        };
    }

    public async Task<MotivationMondayWithQuote> GetTodayMotivationMonday()
    {
        var motivationMonday = await _appDbContext.MotivationMondayWithQuotes
            .OrderByDescending(x => x.QuoteDate)
            .FirstOrDefaultAsync();

        if (motivationMonday == null)
        {
            throw new Exception("No MotivationMonday Found For Today");
        }

        return motivationMonday;
    }
}
