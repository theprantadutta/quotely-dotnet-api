using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using quotely_dotnet_api.Interfaces;

namespace quotely_dotnet_api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[ApiVersion("1.0")]
public class AuthorController(ILogger<AuthorController> logger, IAuthorService authorService)
    : Controller
{
    private readonly ILogger<AuthorController> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly IAuthorService _authorService =
        authorService ?? throw new ArgumentNullException(nameof(authorService));

    [HttpGet]
    public async Task<IActionResult> GetAllAuthors(
        string? search,
        int pageNumber = 1,
        int pageSize = 10,
        bool getAllRows = false
    )
    {
        try
        {
            return Ok(await _authorService.GetAllAuthors(pageNumber, pageSize, getAllRows, search));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Something Went Wrong When Getting Authors");
            throw;
        }
    }
}
