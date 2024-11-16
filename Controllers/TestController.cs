using Microsoft.AspNetCore.Mvc;
using quotely_dotnet_api.Interfaces;

namespace quotely_dotnet_api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TestController(IUtilityService utilityService): Controller
{
    private  readonly IUtilityService _utilityService
        = utilityService ?? throw new ArgumentNullException(nameof(utilityService));

    [HttpGet]
    public async Task<IActionResult> GetWikiImage(string url)
    {
        return Ok(await _utilityService.GetWikipediaThumbnailAsync(url));
    }
}