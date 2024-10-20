using Microsoft.EntityFrameworkCore;
using quotely_dotnet_api.Contexts;
using quotely_dotnet_api.Dtos;
using quotely_dotnet_api.Interfaces;

namespace quotely_dotnet_api.Services;

public class QuoteService(AppDbContext appDbContext) : IQuoteService
{
    private readonly AppDbContext _appDbContext =
        appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));

    public async Task<QuoteResponseDto> GetAllQuotes(int pageNumber, int pageSize, bool getAllRows)
    {
        if (getAllRows)
        {
            var allQuoteRows = await _appDbContext.Quotes.ToListAsync();
            return new QuoteResponseDto()
            {
                Quotes = allQuoteRows,
                Pagination = new PaginationDto()
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItemCount = allQuoteRows.Count,
                }
            };
        }

        var totalItemCount = await _appDbContext.Quotes.CountAsync();

        var query = _appDbContext
            .Quotes.AsSplitQuery()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        var allQuotes = await query.ToListAsync();

        return new QuoteResponseDto()
        {
            Quotes = allQuotes,
            Pagination = new PaginationDto()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItemCount = totalItemCount
            }
        };
    }
}
