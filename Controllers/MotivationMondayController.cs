using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using quotely_dotnet_api.Interfaces;

namespace quotely_dotnet_api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[ApiVersion("1.0")]
public class MotivationMondayController(
    IMotivationMondayService motivationMondayService,
    ILogger<QuoteOfTheDayController> logger
) : Controller
{
    private readonly IMotivationMondayService _motivationMondayService =
        motivationMondayService ?? throw new ArgumentNullException(nameof(motivationMondayService));

    private readonly ILogger<QuoteOfTheDayController> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    [HttpGet]
    public async Task<IActionResult> GetAllMotivationMonday(
        int pageNumber = 1,
        int pageSize = 10,
        bool getAllRows = false
    )
    {
        try
        {
            return Ok(
                await _motivationMondayService.GetAllMotivationMonday(
                    pageNumber,
                    pageSize,
                    getAllRows
                )
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Something Went Wrong When Getting Motivation Monday List");
            throw;
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetTodayMotivationMonday()
    {
        try
        {
            return Ok(await _motivationMondayService.GetTodayMotivationMonday());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Something Went Wrong When Getting Motivation Monday");
            throw;
        }
    }
}
