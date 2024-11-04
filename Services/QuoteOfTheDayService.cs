using Microsoft.EntityFrameworkCore;
using quotely_dotnet_api.Contexts;
using quotely_dotnet_api.Dtos;
using quotely_dotnet_api.Entities;
using quotely_dotnet_api.Interfaces;
using quotely_dotnet_api.Views;

namespace quotely_dotnet_api.Services;

public class QuoteOfTheDayService(AppDbContext appDbContext) : IQuoteOfTheDayService
{
    private readonly AppDbContext _appDbContext = appDbContext
                                                  ?? throw new ArgumentNullException(nameof(appDbContext));
    
    public async Task<QuoteOfTheDayResponseDto> GetAllQuoteOfTheDay(
        int pageNumber, 
        int pageSize, 
        bool getAllRows
    )
    {
        if (getAllRows)
        {
            var allQuoteOfTheDayWithQuotes = await _appDbContext.QuoteOfTheDayWithQuotes.ToListAsync();
            return new QuoteOfTheDayResponseDto()
            {
                QuoteOfTheDayWithQuotes = allQuoteOfTheDayWithQuotes,
                Pagination = new PaginationDto()
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItemCount = allQuoteOfTheDayWithQuotes.Count,
                }
            };
        }

        var totalItemCount = await _appDbContext.QuoteOfTheDayWithQuotes.CountAsync();

        var query = _appDbContext
            .QuoteOfTheDayWithQuotes
            .OrderBy(_ => Guid.NewGuid()) 
            .AsSplitQuery()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        var allQuoteOfTheDay = await query.ToListAsync();

        return new QuoteOfTheDayResponseDto()
        {
            QuoteOfTheDayWithQuotes = allQuoteOfTheDay,
            Pagination = new PaginationDto()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItemCount = totalItemCount
            }
        };
    }
}