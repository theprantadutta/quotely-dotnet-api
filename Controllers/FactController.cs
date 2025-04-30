using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using quotely_dotnet_api.Interfaces;

namespace quotely_dotnet_api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[ApiVersion("1.0")]
public class FactController(ILogger<FactController> logger, IFactService factService) : Controller
{
    private readonly ILogger<FactController> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly IFactService _factService =
        factService ?? throw new ArgumentNullException(nameof(factService));

    public async Task<IActionResult> GetAllAiFacts(
        [FromQuery] string? factCategories,
        [FromQuery] string? aiProviders,
        bool getAllRows = false,
        int pageNumber = 1,
        int pageSize = 10
    )
    {
        try
        {
            // Split the string by commas to get a list
            var categoriesList = factCategories?.Split(',').ToList() ?? [];
            var aiProviderList = aiProviders?.Split(',').ToList() ?? [];

            return Ok(
                await _factService.GetAiFacts(
                    categoriesList,
                    aiProviderList,
                    getAllRows,
                    pageNumber,
                    pageSize
                )
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Something Went Wrong When Getting facts");
            throw;
        }
    }

    public async Task<IActionResult> GetAllAiFactCategories()
    {
        try
        {
            return Ok(await _factService.GetAiFactCategories());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Something Went Wrong When Getting fact categories");
            throw;
        }
    }

    public async Task<IActionResult> GetAllAiFactProviders()
    {
        try
        {
            return Ok(await _factService.GetAiFactProviders());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Something Went Wrong When Getting fact providers");
            throw;
        }
    }
}
