using Microsoft.EntityFrameworkCore;
using quotely_dotnet_api.Contexts;
using quotely_dotnet_api.Dtos;
using quotely_dotnet_api.Interfaces;

namespace quotely_dotnet_api.Services;

public class TagService(AppDbContext appDbContext) : ITagService
{
    private readonly AppDbContext _appDbContext =
        appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    
    public async Task<TagResponseDto> GetAllTags(int pageNumber, int pageSize, bool getAllRows)
    {
        if (getAllRows)
        {
            var allTagRows = await _appDbContext.Tags.ToListAsync();
            return new TagResponseDto()
            {
                Tags = allTagRows,
                Pagination = new PaginationDto()
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItemCount = allTagRows.Count,
                }
            };
        }

        var totalItemCount = await _appDbContext.Quotes.CountAsync();

        var query = _appDbContext
            .Tags.AsSplitQuery()
            .OrderBy(x =>x.DateAdded)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        var allTags = await query.ToListAsync();

        return new TagResponseDto()
        {
            Tags = allTags,
            Pagination = new PaginationDto()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItemCount = totalItemCount
            }
        };
    }
}