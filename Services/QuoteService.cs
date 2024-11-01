using Microsoft.EntityFrameworkCore;
using quotely_dotnet_api.Contexts;
using quotely_dotnet_api.Dtos;
using quotely_dotnet_api.Interfaces;

namespace quotely_dotnet_api.Services;

public class QuoteService(AppDbContext appDbContext) : IQuoteService
{
    private readonly AppDbContext _appDbContext =
        appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));

    public async Task<QuoteResponseDto> GetAllQuotes(int pageNumber, int pageSize, bool getAllRows, List<string>? tags)
    {
        var query = _appDbContext.Quotes.AsQueryable();

        if (tags is { Count: > 0 })
        {
            // Filter quotes where any tag in the provided tags list matches any tag in the quote's Tags array
            query = query.Where(q => q.Tags.Any(tags.Contains));
        }

        // Apply random ordering
        query = query.OrderBy(_ => Guid.NewGuid());

        if (getAllRows)
        {
            var allQuoteRows = await query.ToListAsync();
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

        var totalItemCount = await query.CountAsync();

        // Apply pagination
        var paginatedQuotes = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new QuoteResponseDto()
        {
            Quotes = paginatedQuotes,
            Pagination = new PaginationDto()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItemCount = totalItemCount
            }
        };
    }
}
