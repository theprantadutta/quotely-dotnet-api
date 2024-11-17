using Microsoft.AspNetCore.Mvc;
using quotely_dotnet_api.Interfaces;

namespace quotely_dotnet_api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TestController(IUtilityService utilityService,
    IFirebaseMessagingClient firebaseMessagingClient): Controller
{
    private  readonly IUtilityService _utilityService
        = utilityService ?? throw new ArgumentNullException(nameof(utilityService));

    private readonly IFirebaseMessagingClient _firebaseMessagingClient
        = firebaseMessagingClient ?? throw new ArgumentNullException(nameof(firebaseMessagingClient));
    
    [HttpGet]
    public async Task<IActionResult> GetWikiImage(string url)
    {
        return Ok(await _utilityService.GetWikipediaThumbnailAsync(url));
    }
    
    [HttpGet]
    public async Task<IActionResult> TestNotification()
    {
        return Ok(await _firebaseMessagingClient.SendNotification("all",
            "Test Notification", "Test About Test Notification", new Dictionary<string, string>()));
    }
}