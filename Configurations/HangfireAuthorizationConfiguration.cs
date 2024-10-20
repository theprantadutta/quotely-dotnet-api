using Hangfire.Dashboard.BasicAuthorization;

namespace quotely_dotnet_api.Configurations;

public class HangfireAuthorizationConfiguration
{
    public static BasicAuthAuthorizationFilter GetBasicAuthFilter()
    {
        return new BasicAuthAuthorizationFilter(
            new BasicAuthAuthorizationFilterOptions
            {
                RequireSsl = false,
                SslRedirect = false,
                LoginCaseSensitive = true,
                Users = new[]
                {
                    new BasicAuthAuthorizationUser { Login = "admin", PasswordClear = "test@123" }
                }
            }
        );
    }
}