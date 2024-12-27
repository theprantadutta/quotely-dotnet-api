using quotely_dotnet_api.Entities;

namespace quotely_dotnet_api.Interfaces;

public interface IApplicationService
{
    Task<ApplicationInfo> GetApplicationInfo();
}
