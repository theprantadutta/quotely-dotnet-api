using Microsoft.EntityFrameworkCore;
using quotely_dotnet_api.Contexts;
using quotely_dotnet_api.Dtos;
using quotely_dotnet_api.Entities;
using quotely_dotnet_api.Interfaces;

namespace quotely_dotnet_api.Services;

public class AuthorService(AppDbContext appDbContext) : IAuthorService
{
    private readonly AppDbContext _appDbContext =
        appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));

    public async Task<AuthorResponseDto> GetAllAuthors(
        int pageNumber,
        int pageSize,
        bool getAllRows,
        string? search
    )
    {
        IQueryable<Author> query = _appDbContext.Authors;

        // Apply search filter if search term is provided
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(a => a.Name.ToLower().Contains(search.ToLower()));
        }

        query = query.OrderBy(_ => Guid.NewGuid());

        if (getAllRows)
        {
            var allAuthorRows = await query.ToListAsync();
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

        var totalItemCount = await query.CountAsync();

        query = query
            .AsSplitQuery()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        var paginatedAuthors = await query.ToListAsync();

        return new AuthorResponseDto()
        {
            Authors = paginatedAuthors,
            Pagination = new PaginationDto()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItemCount = totalItemCount
            }
        };
    }

    public async Task<Author?> GetAuthorDetails(string authorSlug)
    {
        return await _appDbContext
            .Authors
            .Where(x => x.Slug == authorSlug)
            .FirstOrDefaultAsync();
    }
}
