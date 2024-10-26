using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using quotely_dotnet_api.Interfaces;

namespace quotely_dotnet_api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[ApiVersion("1.0")]
public class TagController(ILogger<QuoteController> logger, ITagService tagService): Controller
{
    private readonly ILogger<QuoteController> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly ITagService _tagService =
        tagService ?? throw new ArgumentNullException(nameof(tagService));

    [HttpGet]
    public async Task<IActionResult> GetAllQuotes(
        int pageNumber = 1,
        int pageSize = 10,
        bool getAllRows = false
    )
    {
        try
        {
            return Ok(await _tagService.GetAllTags(pageNumber, pageSize, getAllRows));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Something Went Wrong When Getting Tags");
            throw;
        }
    }
}