using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using quotely_dotnet_api.Interfaces;

namespace quotely_dotnet_api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
[ApiVersion("1.0")]
public class ApplicationController(
    ILogger<ApplicationController> logger,
    IApplicationService applicationService
) : Controller
{
    private readonly ILogger<ApplicationController> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly IApplicationService _applicationService =
        applicationService ?? throw new ArgumentNullException(nameof(applicationService));

    [HttpGet]
    public async Task<IActionResult> GetApplicationInfo()
    {
        try
        {
            return Ok(await _applicationService.GetApplicationInfo());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Something Went Wrong when getting application info");
            throw;
        }
    }
}
