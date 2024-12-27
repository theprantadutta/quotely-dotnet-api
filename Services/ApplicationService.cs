using Microsoft.EntityFrameworkCore;
using quotely_dotnet_api.Contexts;
using quotely_dotnet_api.Entities;
using quotely_dotnet_api.Interfaces;

namespace quotely_dotnet_api.Services;

public class ApplicationService(AppDbContext appDbContext) : IApplicationService
{
    private readonly AppDbContext _appDbContext =
        appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));

    public async Task<ApplicationInfo> GetApplicationInfo()
    {
        var applicationInfo = await _appDbContext.ApplicationInfos.FirstOrDefaultAsync();

        if (applicationInfo == null)
        {
            throw new Exception("No application info found");
        }

        return applicationInfo;
    }
}
