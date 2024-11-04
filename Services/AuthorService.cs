using Microsoft.EntityFrameworkCore;
using quotely_dotnet_api.Contexts;
using quotely_dotnet_api.Dtos;
using quotely_dotnet_api.Interfaces;

namespace quotely_dotnet_api.Services;

public class AuthorService(AppDbContext appDbContext) : IAuthorService
{
    private readonly AppDbContext _appDbContext =
        appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));

    public async Task<AuthorResponseDto> GetAllAuthors(
        int pageNumber,
        int pageSize,
        bool getAllRows
    )
    {
        if (getAllRows)
        {
            var allAuthorRows = await _appDbContext.Authors.ToListAsync();
            return new AuthorResponseDto()
            {
                Authors = allAuthorRows,
                Pagination = new PaginationDto()
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItemCount = allAuthorRows.Count,
                }
            };
        }

        var totalItemCount = await _appDbContext.Authors.CountAsync();

        var query = _appDbContext
            .Authors
            .OrderBy(_ => Guid.NewGuid()) 
            .AsSplitQuery()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        var allAuthors = await query.ToListAsync();

        return new AuthorResponseDto()
        {
            Authors = allAuthors,
            Pagination = new PaginationDto()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItemCount = totalItemCount
            }
        };
    }

}
