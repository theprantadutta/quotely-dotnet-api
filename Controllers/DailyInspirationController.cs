using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using quotely_dotnet_api.Interfaces;

namespace quotely_dotnet_api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[ApiVersion("1.0")]
public class DailyInspirationController(
    IDailyInspirationService dailyInspirationService,
    ILogger<QuoteOfTheDayController> logger
) : Controller
{
    private readonly IDailyInspirationService _dailyInspirationService =
        dailyInspirationService ?? throw new ArgumentNullException(nameof(dailyInspirationService));

    private readonly ILogger<QuoteOfTheDayController> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    [HttpGet]
    public async Task<IActionResult> GetAllDailyInspiration(
        int pageNumber = 1,
        int pageSize = 10,
        bool getAllRows = false
    )
    {
        try
        {
            return Ok(
                await _dailyInspirationService.GetAllDailyInspiration(
                    pageNumber,
                    pageSize,
                    getAllRows
                )
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Something Went Wrong When Getting Daily Inspiration");
            throw;
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetTodayDailyInspiration()
    {
        try
        {
            return Ok(await _dailyInspirationService.GetTodayDailyInspiration());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Something Went Wrong When Getting Daily Inspiration");
            throw;
        }
    }
}
