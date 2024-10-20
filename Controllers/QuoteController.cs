using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using quotely_dotnet_api.Interfaces;

namespace quotely_dotnet_api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[ApiVersion("1.0")]
public class QuoteController(ILogger<QuoteController> logger, IQuoteService quoteService)
    : Controller
{
    private readonly ILogger<QuoteController> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly IQuoteService _quoteService =
        quoteService ?? throw new ArgumentNullException(nameof(quoteService));

    [HttpGet]
    public async Task<IActionResult> GetAllQuotes(
        int pageNumber = 1,
        int pageSize = 10,
        bool getAllRows = false
    )
    {
        try
        {
            return Ok(await _quoteService.GetAllQuotes(pageNumber, pageSize, getAllRows));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Something Went Wrong When Getting Quotes");
            throw;
        }
    }
}
