using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using quotely_dotnet_api.Interfaces;

namespace quotely_dotnet_api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[ApiVersion("1.0")]
public class QuoteOfTheDayController(
    IQuoteOfTheDayService quoteOfTheDayService, 
    ILogger<QuoteOfTheDayController> logger) : Controller
{
    private readonly IQuoteOfTheDayService _quoteOfTheDayService = quoteOfTheDayService
        ?? throw new ArgumentNullException(nameof(quoteOfTheDayService));
    
    private  readonly ILogger<QuoteOfTheDayController> _logger = logger
        ?? throw new ArgumentNullException(nameof(logger));
    
    [HttpGet]
    public async Task<IActionResult> GetAllQuoteOfTheDay(
        int pageNumber = 1,
        int pageSize = 10,
        bool getAllRows = false
    )
    {
        try
        {
            return Ok(await _quoteOfTheDayService.GetAllQuoteOfTheDay(pageNumber, pageSize, getAllRows));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Something Went Wrong When Getting Quote of the day");
            throw;
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTodayQuoteOfTheDay()
    {
        try
        {
            return Ok(await _quoteOfTheDayService.GetTodayQuoteOfTheDay());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Something Went Wrong When Getting Today Quote");
            throw;
        }
    }
}