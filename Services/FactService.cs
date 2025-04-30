using Microsoft.EntityFrameworkCore;
using quotely_dotnet_api.Contexts;
using quotely_dotnet_api.Dtos;
using quotely_dotnet_api.Interfaces;

namespace quotely_dotnet_api.Services;

public class FactService(AppDbContext appDbContext) : IFactService
{
    private readonly AppDbContext _appDbContext =
        appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));

    public async Task<AiFactResponseDto> GetAiFacts(
        List<string> categories,
        List<string> aiProviders,
        bool getAllRows,
        int pageNumber = 1,
        int pageSize = 10
    )
    {
        // Start with a base query
        var query = _appDbContext.AiFacts.AsQueryable();

        // Apply category filter if provided
        if (categories?.Count > 0)
        {
            query = query.Where(f => categories.Contains(f.AiFactCategory));
        }

        // Apply provider filter if provided
        if (aiProviders?.Count > 0)
        {
            query = query.Where(f => aiProviders.Contains(f.Provider));
        }

        // For getAllRows, return everything (filtered but unpaginated)
        if (getAllRows)
        {
            var allFacts = await query.ToListAsync();
            return new AiFactResponseDto()
            {
                Facts = allFacts,
                Pagination = new PaginationDto()
                {
                    PageNumber = 1,
                    PageSize = allFacts.Count,
                    TotalItemCount = allFacts.Count
                }
            };
        }

        // Get total count before pagination
        var totalItemCount = await query.CountAsync();

        // Apply pagination
        var paginatedFacts = await query
            .OrderByDescending(f => f.DateAdded) // Default ordering by newest
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new AiFactResponseDto()
        {
            Facts = paginatedFacts,
            Pagination = new PaginationDto()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItemCount = totalItemCount,
            }
        };
    }

    public async Task<List<string>> GetAiFactCategories()
    {
        return await _appDbContext.AiFacts.Select(x => x.AiFactCategory).Distinct().ToListAsync();
    }

    public async Task<List<string>> GetAiFactProviders()
    {
        return await _appDbContext.AiFacts.Select(x => x.Provider).Distinct().ToListAsync();
    }
}
